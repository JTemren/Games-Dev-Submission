using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public sealed class AiController : MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public float startWaitTime = 4;
        public float timeToRotate = 2;
        public float speedWalk = 6;
        public float speedRun = 9;
        [SerializeField] private Animator animator;
 
        public float viewRadius = 15;
        public float viewAngle = 90;
        public LayerMask playerMask;
        public LayerMask obstacleMask;
        public float meshResolution = 1.0f;
        public int edgeIterations = 4;
        public float edgeDistance = 0.5f;
        
        public Transform[] waypoints;
        private int _currentWaypointIndex;

        private Vector3 _playerLastPosition = Vector3.zero;
        private Vector3 _playerPosition;

        private float _waitTime;
        private float _timeToRotate;
        [SerializeField] private bool playerInRange;
        [SerializeField] private bool playerNear;
        [SerializeField] private bool isPatrol;
        [SerializeField] private bool caughtPlayer;

        private void Start()
        {
            animator = GetComponent<Animator>();
            _playerPosition = Vector3.zero;
            isPatrol = true;
            caughtPlayer = false;
            playerInRange = false;
            playerNear = false;
            _waitTime = startWaitTime;
            _timeToRotate = timeToRotate;
 
            _currentWaypointIndex = 0;
            navMeshAgent = GetComponent<NavMeshAgent>();
 
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speedWalk;
            navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
        }
 
        private void Update()
        {
            EnvironmentView();
 
            if (!isPatrol)
            {
                Chasing();
            }
            else
            {
                Patrolling();
            }
        }
 
        private void Chasing()
        {
            playerNear = false;
            _playerLastPosition = Vector3.zero;
            animator.SetBool("IsMoving",true);
 
            if (!caughtPlayer)
            {
                Move(speedRun);
                navMeshAgent.SetDestination(_playerPosition);
            }

            if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return;
            if (!(_waitTime <= 0) || caughtPlayer || !(Vector3.Distance(transform.position,
                    GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f))
            {
                if (Vector3.Distance(transform.position,
                        GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    Stop();

                _waitTime -= Time.deltaTime;
            }
            else
            {
                animator.SetBool("IsMoving", false);
                isPatrol = false;
                playerNear = true;
                Move(speedWalk);
                _timeToRotate = timeToRotate;
                _waitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
            }
        }
 
        private void Patrolling()
        {
            
            if (playerNear)
            {
                if (_timeToRotate <= 0)
                {
                    Move(speedWalk);
                    LookingPlayer(_playerLastPosition);
                }
                else
                {
                    Stop();
                    _timeToRotate -= Time.deltaTime;
                }
            }
            else
            {
                playerNear = false;
                _playerLastPosition = Vector3.zero;
                navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
                if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return;
                if (_waitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    _waitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    _waitTime -= Time.deltaTime;
                }
            }
        }

        private void NextPoint()
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
        }

        private void Stop()
        {
            animator.SetBool("IsMoving",false);
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;
        }

        private void Move(float speed)
        {
            animator.SetBool("IsMoving",true);
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
        }
 
        void CaughtPlayer()
        {
            caughtPlayer = true;
            Debug.Log("attacking player");
        }
 
        void LookingPlayer(Vector3 player)
        {
            navMeshAgent.SetDestination(player);
            if (!(Vector3.Distance(transform.position, player) <= 0.3)) return;
            if (_waitTime <= 0)
            {
                playerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
                _waitTime = startWaitTime;
                _timeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                _waitTime -= Time.deltaTime;
            }
        }

        private void EnvironmentView()
        {
            Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

            foreach (var t in playerInRange)
            {
                Transform player = t.transform;
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                if (!(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2))
                {
                }
                else
                {
                    var dstToPlayer = Vector3.Distance(transform.position, player.position);
                    if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                    {
                        this.playerInRange = true;
                        isPatrol = false;
                    }
                    else
                    {
                        this.playerInRange = false;
                    }
                }

                if (Vector3.Distance(transform.position, player.position) > viewRadius)
                {
                    this.playerInRange = false;
                }
                if (this.playerInRange)
                {
                    _playerPosition = player.transform.position;
                }
            }
        }
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            if (playerInRange != false)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, _playerPosition);
            }
        }
    }
}