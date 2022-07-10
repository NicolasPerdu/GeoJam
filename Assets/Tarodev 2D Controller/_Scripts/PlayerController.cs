using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TarodevController {
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// Right now it only contains movement and jumping, but it should be pretty easy to expand... I may even do it myself
    /// if there's enough interest. You can play and compete for best times here: https://tarodev.itch.io/
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/GqeHHnhHpz
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController {

        public const float VERTICAL_DEATH_LINE = -20F;
        
        [HideInInspector]public int facingDirection = 1;

        public bool isActivePlayer => MasterControl.main.activeAvatar != null && MasterControl.main.activeAvatar.transform == transform;

        public bool debugBypassNixJump = false;

        // Public for external hooks
        
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => _colDown;
        public NPC targetNPC;
        public SpriteRenderer renderer2;
        private float interactionRadius = 2f;
        private Vector2 move;


        private bool freezingMovement = false;
        private bool freezingGravity = false;
        private bool freezingJump = false;
        private static bool canJump = false;
        private Vector3 _lastPosition;
        private float _currentHorizontalSpeed, _currentVerticalSpeed;
        

        // This is horrible, but for some reason colliders are not fully established when update starts...
        private bool _active;
        void Awake() => Invoke(nameof(Activate), 0.5f);
        void Activate() =>  _active = true;

        void Start()
        {
            if (debugBypassNixJump) canJump = true;
        }
        
        private void Update() {
            if(!_active) return;
            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            GatherInput();
            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
            FallDeathCheck();
        }
        public void ToggleCanJump()
        {
            canJump = true;
        }

        #region Gather Input

        private void GatherInput() {
            if (isActivePlayer)
            {
                	Input = new FrameInput {
                		JumpDown = freezingJump || !canJump ? false : UnityEngine.Input.GetButtonDown("Jump"),
                		JumpUp = freezingJump || !canJump ? false : UnityEngine.Input.GetButtonUp("Jump"),
                		X = freezingMovement ? 0 : UnityEngine.Input.GetAxisRaw("Horizontal"),
                    	Dialog = UnityEngine.Input.GetButtonDown("Fire1")
                };
                if (Input.JumpDown) {
                    _lastJumpPressed = Time.time;
                }
            }
            else
            {
                Input = new FrameInput {};
            }
        }

        #endregion

        #region Collisions

        public void PauseMovement(float seconds) => StartCoroutine(FreezeMovementOnTimer(seconds));
        public void PauseGravity(float seconds) => StartCoroutine(FreezeGravityOnTimer(seconds));
        public void PauseJumping(float seconds) => StartCoroutine(FreezeJumpingOnTimer(seconds));

        [Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
        [SerializeField] public LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        private bool _colUp, _colRight, _colDown, _colLeft;

        public bool ColUp => _colUp;
        public bool ColDown => _colDown;
        public bool ColLeft => _colLeft;
        public bool ColRight => _colRight;

        private float _timeLeftGrounded;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks() {
            // Generate ray ranges. 
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var groundedCheck = RunDetection(_raysDown);
            if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!_colDown && groundedCheck) {
                _coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;
            }

            _colDown = groundedCheck;

            // The rest
            _colUp = RunDetection(_raysUp);
            _colLeft = RunDetection(_raysLeft);
            _colRight = RunDetection(_raysRight);

            bool RunDetection(RayRange range) {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanged() {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position, _characterBounds.size);

            _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }

        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range) {
            for (var i = 0; i < _detectorCount; i++) {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos() {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying) {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft }) {
                    foreach (var point in EvaluateRayPositions(range)) {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + move, _characterBounds.size);
        }

        #endregion


        #region Walk

        [Header("WALKING")] [SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;


        private void CalculateWalk() {
            if(Input.Dialog) {
                /*Debug.Log("Dialog !");
                if (targetDialog != null && targetDialog.choicesGenerated) {
                    targetDialog.pushButton();
                } else {
                    findTargetNPC();
                    move = Vector2.zero;

                    if (targetDialog != null) {
                        if (targetDialog.dialogEnabled) {
                            continuStory();
                        } else {
                            CheckForNearbyNPCInky();
                        }
                    }
                }*/
            }
            

            if (Input.X != 0) {
                // Set horizontal move speed
                _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;

                facingDirection = Input.X > 0 ? 1 : -1;
            }
            else {
                // No input. Let's slow the character down
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft) {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")] [SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity() {
            if (freezingGravity)
            {

            }
            else if (_colDown) {
                // Move out of the ground
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
            }
            else {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")] [SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        [HideInInspector]public Vector2 modSpeed = Vector2.zero;


        public void ResetValues()
        {
            modSpeed = Vector2.zero;
        }

        private void FallDeathCheck()
        {
            if (transform.position.y > VERTICAL_DEATH_LINE) return;


            foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>())
            {
                if (checkpoint.Active)
                {
                    checkpoint.Respawn();
                    break;
                }
            }
        }

        private void CalculateJumpApex() {
            if (!_colDown) {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else {
                _apexPoint = 0;
            }
        }

        private void CalculateJump() {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (Input.JumpDown && CanUseCoyote || HasBufferedJump) {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            else {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0) {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            if (_colUp) {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        #endregion

        #region Move

        [Header("MOVE")] [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter() {
            var pos = transform.position;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit) {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            var positionToMoveTo = transform.position;
            for (int i = 1; i < _freeColliderIterations; i++) {
                // increment to check all but furthestPoint - we did that already
                var t = (float)i / _freeColliderIterations;
                var posToTry2D = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry2D, _characterBounds.size, 0, _groundLayer)) {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1) {
                        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = new Vector3(posToTry2D.x, posToTry2D.y, transform.position.z);
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other) {
            //Debug.Log("Trigger enter!");
            Teleporter tp = other.gameObject.GetComponent<Teleporter>();
            if (tp) {
                //Debug.Log("Hit Teleport: " + other.gameObject.name);
                transform.position = new Vector3(tp.DestinationOnLane.position.x, tp.DestinationOnLane.position.y, tp.DestinationLane.transform.localPosition.z);
                _groundLayer = tp.DestinationLane.GroundLayer;
            }

            Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();
            if (cp) {
                //Debug.Log("Hit CP!");
                cp.Activate();
            }
        }

        void OnColliderEnter2D(Collision2D c) {
            if (c.gameObject.tag == "Bouncer")
            {
                Vector2 nAverage = Vector3.zero;
                int i = 0;
                foreach (ContactPoint2D contact in c.contacts)
                {
                    nAverage += contact.normal;
                    i++;
                }

                nAverage /= i;

                _currentHorizontalSpeed = nAverage.x * 40;
            }
        }

        IEnumerator FreezeMovementOnTimer(float seconds)
        {
            freezingMovement = true;
            yield return new WaitForSeconds(seconds);
            freezingMovement = false;
        }

        IEnumerator FreezeJumpingOnTimer(float seconds)
        {
            freezingJump = true;
            yield return new WaitForSeconds(seconds);
            freezingJump = false;
        }

        IEnumerator FreezeGravityOnTimer(float seconds)
        {
            _currentVerticalSpeed = 0;
            freezingGravity = true;
            freezingMovement = true;
            yield return new WaitForSeconds(seconds);
            freezingGravity = false;
            freezingMovement = false;
        }

        public void DisableSelf()
        {
            gameObject.SetActive(false);
        }

        public void EnableSelf()
        {
            gameObject.SetActive(true);
        }
    }
}