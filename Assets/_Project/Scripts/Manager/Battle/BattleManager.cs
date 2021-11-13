using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace HOTS.Manager.Battle
{
    /// <summary>
    /// Class handles the battle sequences.
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        /// <summary>
        /// Contains the battle event system.
        /// </summary>
        [Inject] private BattleEventSystem _battleEventSystem;

        /// <summary>
        /// Defines the amount of seconds which defines
        /// how long the battle start sequence will be.
        /// </summary>
        private const float _startBattleSequenceDuration = 2.5f;

        /// <summary>
        /// Contains the positions in the battle circle
        /// for each player up to four.
        /// </summary>
        private readonly int[] _playerPositions = new[] { 36 * 9, 36 * 8, 36 * 7, 36 * 6 };

        /// <summary>
        /// Contains the positions in the battle circle
        /// for each enemy up to four.
        /// </summary>
        private readonly int[] _enemyPositions = new[] { 36 * 4, 36 * 3, 36 * 2, 36 * 1 };

        /// <summary>
        /// Contains the radius of the battle ground.
        /// </summary>
        private float _battleGroundRadius;

        /// <summary>
        /// Defines whether the battle has started already
        /// or not.
        /// </summary>
        private bool _battleStarted;

        /// <summary>
        /// Contains the template for the battle ground.
        /// </summary>
        [SerializeField] private GameObject _battleGroundTemplate;

        /// <summary>
        /// Contains the player.
        /// </summary>
        private GameObject _player;

        /// <summary>
        /// Contains the enemy.
        /// </summary>
        private GameObject _enemy;

        /// <summary>
        /// Contains the current used battle ground.
        /// </summary>
        private GameObject _battleGround;


        private void Start()
        {
            _battleEventSystem.onBattleBegin += OnBattleBegin;
            _battleEventSystem.onBattleEnd += OnBattleEnd;

            // The scale of the circle divided by two is
            // equal to the radius of the circle
            _battleGroundRadius = _battleGroundTemplate.transform.localScale.x / 2;
        }

        /// <summary>
        /// Handles the battle begin event.
        /// </summary>
        /// <param name="player">Player in the battle</param>
        /// <param name="enemy">Enemy in the battle</param>
        private void OnBattleBegin(GameObject player, GameObject enemy)
        {
            // Prevent multiple activations
            if (_battleStarted) { return; }

            _player = player;
            _enemy = enemy;
            _battleStarted = true;

            StartCoroutine("StartBattleSequence");
        }

        /// <summary>
        /// Starts the battle start sequence and places each
        /// enemy and player correctly.
        /// </summary>
        private void StartBattleSequence()
        {
            // Create battle ground around players current position
            var pos = _player.transform.position;
            pos.y = _battleGroundTemplate.transform.position.y;
            _battleGround = Instantiate(_battleGroundTemplate, pos, Quaternion.identity);

            // Deactivate players movement
            _player.GetComponent<PlayerInput>().DeactivateInput();

            // Move player to desired battle position
            StartCoroutine(SetPlayerBattlePosition(_player, 0, _battleGround));

            // Move enemy to desired battle position
            StartCoroutine(SetEnemyBattlePosition(_enemy, 0, _battleGround));
        }

        /// <summary>
        /// Sets the battle position for <paramref name="player"/>.
        /// </summary>
        /// <param name="player">Player to position</param>
        /// <param name="index">Place index for <paramref name="player"/></param>
        /// <param name="battleGround">Battle ground in which <paramref name="player"/> should be moved</param>
        /// <returns>Movement to battle place</returns>
        private IEnumerator SetPlayerBattlePosition(GameObject player, int index, GameObject battleGround)
        {
            // Get position in battle
            var degree = _playerPositions[index];
            var position = GetBattlePosition(battleGround.transform.position, degree, _battleGroundRadius);
            position.y = player.transform.position.y;

            // Move player to battle position
            Debug.Log($"Move player #{index} from {player.transform.position} to {position}");
            return MoveObjectSmooth(player, position, _startBattleSequenceDuration);
        }

        /// <summary>
        /// Sets the battle position for <paramref name="enemy"/>.
        /// </summary>
        /// <param name="enemy">Enemy to position</param>
        /// <param name="index">Place index for <paramref name="enemy"/></param>
        /// <param name="battleGround">Battle ground in which <paramref name="enemy"/> should be moved</param>
        /// <returns>Movement to battle place</returns>
        private IEnumerator SetEnemyBattlePosition(GameObject enemy, int index, GameObject battleGround)
        {
            // Get position in battle
            var degree = _enemyPositions[index];
            var position = GetBattlePosition(battleGround.transform.position, degree, _battleGroundRadius);
            position.y = enemy.transform.position.y;

            // Move player to battle position
            Debug.Log($"Move enemy #{index} from {enemy.transform.position} to {position}");
            return MoveObjectSmooth(enemy, position, _startBattleSequenceDuration);
        }

        /// <summary>
        /// Moves <paramref name="gameObject"/> smoothly to
        /// <paramref name="finalPosition"/> in given
        /// <paramref name="time"/>.
        /// </summary>
        /// <param name="gameObject">Object to move</param>
        /// <param name="finalPosition">Final position of <paramref name="gameObject"/></param>
        /// <param name="time">Duration in seconds</param>
        /// <returns>Movement to battle place</returns>
        private IEnumerator MoveObjectSmooth(GameObject gameObject, Vector3 finalPosition, float time)
        {
            var startingPos = gameObject.transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < time)
            {
                gameObject.transform.position = Vector3.Lerp(startingPos, finalPosition, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Returns the battle position for a circle
        /// with given <paramref name="radius"/> at
        /// specified <paramref name="degree"/> from
        /// a given <paramref name="circlePosition"/>.
        /// </summary>
        /// <param name="circlePosition">Position of the circle</param>
        /// <param name="degree">Degree to use</param>
        /// <param name="radius">Radius of the circle</param>
        /// <returns></returns>
        private Vector3 GetBattlePosition(Vector3 circlePosition, int degree, float radius)
        {
            var radians = degree * Mathf.Deg2Rad;
            return new Vector3
            {
                x = circlePosition.x + Mathf.Cos(radians) * radius,
                y = 0,
                z = circlePosition.z + Mathf.Sin(radians) * radius
            };
        }


        /// <summary>
        /// Handles the battle end event.
        /// </summary>
        private void OnBattleEnd()
        {
            StartCoroutine("EndBattleSequence");
            _battleStarted = false;
        }

        /// <summary>
        /// Starts the battle end sequence and clears everything.
        /// </summary>
        private void EndBattleSequence()
        {
            // Remove battle ground
            Destroy(_battleGround);

            // Activate players movement
            _player.GetComponent<PlayerInput>().ActivateInput();
        }
    }

}