using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem.UI;

namespace Match3
{
    public class Match3 : MonoBehaviour
    {
        [SerializeField] int width = 8;
        [SerializeField] int height = 8;
        [SerializeField] float cellSize = 1f;
        [SerializeField] Vector3 originPosition = Vector3.zero;
        [SerializeField] bool debug = true;

        [SerializeField] Gem gemPrefab;
        [SerializeField] GemType[] gemTypes;
        [SerializeField] Ease ease = Ease.InQuad;
        [SerializeField] GameObject explosion;

        public TextMeshProUGUI matchProgressText; // Reference to the UI Text for progress
        public GemType targetGemType; // The specific gem type the player needs to match 20 of

        private GemMatchTracker gemMatchTracker; // GemMatchTracker instance

        InputReader inputReader;
        AudioManager audioManager;

        GridSystem2D<GridObject<Gem>> grid;

        Vector2Int selectedGem = Vector2Int.one * -1;

        //grabs the dialogue script -Astraea
        public Dialogue dialogue;

        bool isProcessingTurn = false; // Flag to prevent multiple selections during processing

        void Start()
        {
            inputReader = GetComponent<InputReader>();
            audioManager = GetComponent<AudioManager>();

            gemMatchTracker = new GemMatchTracker(targetGemType); // Track the specific gem type

            InitializeGrid();
            inputReader.Fire += OnSelectGem;

            UpdateMatchProgressText(); // Initialize the UI text with current progress
        }

        void UpdateMatchProgressText()
        {
            //edited text display for updated UI -Astraea
            int matchesLeft = 20 - gemMatchTracker.MatchesLeftForWin();
            matchProgressText.text = $"{matchesLeft}/20";
        }

        void OnDestroy()
        {
            inputReader.Fire -= OnSelectGem;
        }

        void OnSelectGem()
        {
            // If a turn is being processed, ignore input
            if (isProcessingTurn) return;

            var gridPos = grid.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("Mouse: " + gridPos);
            //gridPos = grid.GetXY(FindObjectOfType<MouseVisual>().transform.position);
            Debug.Log("Controller: "+gridPos);

            if (!IsValidPosition(gridPos) || IsEmptyPosition(gridPos)) return;

            if (selectedGem == gridPos)
            {
                DeselectGem();
                audioManager.PlayDeselect();
            }
            else if (selectedGem == Vector2Int.one * -1)
            {
                SelectGem(gridPos);
                
                audioManager.PlayClick();
            }
            else if (AreAdjacent(selectedGem, gridPos))
            {
                // Start processing the turn
                isProcessingTurn = true;
                StartCoroutine(RunGameLoop(selectedGem, gridPos));
            }
            else
            {
                DeselectGem();
                audioManager.PlayNoMatch();
            }
        }

        bool AreAdjacent(Vector2Int posA, Vector2Int posB)
        {
            int deltaX = Mathf.Abs(posA.x - posB.x);
            int deltaY = Mathf.Abs(posA.y - posB.y);
            return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
        }

        IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            yield return StartCoroutine(SwapGems(gridPosA, gridPosB));

            List<Vector2Int> matches = FindMatches();

            if (matches.Count == 0)
            {
                audioManager.PlayNoMatch();
                yield return StartCoroutine(SwapGems(gridPosB, gridPosA));
            }
            else
            {
                audioManager.PlayMatch();

                while (matches.Count > 0)
                {
                    yield return StartCoroutine(ExplodeGems(matches));

                    // Update the progress text after matches are tracked
                    UpdateMatchProgressText();

                    // Check if the player has won after tracking matches
                    if (gemMatchTracker.HasPlayerWon())
                    {
                        Debug.Log("Player has won the level!");
                        //matchProgressText.text = "You won!";

                        LayoutSwitch layoutSwitch = GetComponent<LayoutSwitch>();
                        layoutSwitch.LoadNextLevel(layoutSwitch.currentLevelIndex+1);
                        yield break;
                    }

                    yield return StartCoroutine(MakeGemsFall());
                    yield return StartCoroutine(FillEmptySpots());

                    yield return new WaitForSeconds(0.2f);
                    matches = FindMatches();

                    if (matches.Count > 0)
                    {
                        audioManager.PlayMatch();
                    }
                }
            }

            DeselectGem();
            isProcessingTurn = false;
        }



        IEnumerator FillEmptySpots()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid.GetValue(x, y) == null)
                    {
                        CreateGem(x, y);
                        audioManager.PlayPop();
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }

        IEnumerator MakeGemsFall()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid.GetValue(x, y) == null)
                    {
                        for (var i = y + 1; i < height; i++)
                        {
                            if (grid.GetValue(x, i) != null)
                            {
                                var gem = grid.GetValue(x, i).GetValue();
                                grid.SetValue(x, y, grid.GetValue(x, i));
                                grid.SetValue(x, i, null);
                                gem.transform
                                    .DOLocalMove(grid.GetWorldPositionCenter(x, y), 0.5f)
                                    .SetEase(ease);
                                audioManager.PlayWoosh();
                                yield return new WaitForSeconds(0.1f);
                                break;
                            }
                        }
                    }
                }
            }
        }

        IEnumerator ExplodeGems(List<Vector2Int> matches)
        {
            audioManager.PlayPop();

            //updates the dialogue text -Astraea
            if (dialogue) { dialogue.MatchMessage(); } else { Debug.Log("No dialogue found."); }

            CountDestroyedGems(matches); // Paul Code

            foreach (var match in matches)
            {
                var gem = grid.GetValue(match.x, match.y).GetValue();

                // Track this match in the GemMatchTracker
                gemMatchTracker.TrackMatch(gem.GetType());

                grid.SetValue(match.x, match.y, null);
                ExplodeVFX(match);
                gem.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f, 1, 0.5f);

                yield return new WaitForSeconds(0.1f);

                Destroy(gem.gameObject, 0.1f);
            }

            // Optionally, print match counts for debugging
            //gemMatchTracker.PrintMatchCounts();
        }

        //gemMatchTracker.Reset(); -this resets gems


        // Get the number of times a specific gem type has been matched
        //int redGemMatches = gemMatchTracker.GetMatchCount(redGemType); // Assuming 'redGemType' is a defined GemType

        void ExplodeVFX(Vector2Int match)
        {
            var fx = Instantiate(explosion, transform);
            fx.transform.position = grid.GetWorldPositionCenter(match.x, match.y);
            Destroy(fx, 5f);
        }

        List<Vector2Int> FindMatches()
        {
            HashSet<Vector2Int> matches = new();

            // Horizontal matches
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width - 2; x++)
                {
                    var gemA = grid.GetValue(x, y);
                    var gemB = grid.GetValue(x + 1, y);
                    var gemC = grid.GetValue(x + 2, y);

                    if (gemA == null || gemB == null || gemC == null) continue;

                    if (gemA.GetValue().GetType() == gemB.GetValue().GetType()
                        && gemB.GetValue().GetType() == gemC.GetValue().GetType())
                    {
                        matches.Add(new Vector2Int(x, y));
                        matches.Add(new Vector2Int(x + 1, y));
                        matches.Add(new Vector2Int(x + 2, y));
                    }
                }
            }

            // Vertical matches
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height - 2; y++)
                {
                    var gemA = grid.GetValue(x, y);
                    var gemB = grid.GetValue(x, y + 1);
                    var gemC = grid.GetValue(x, y + 2);

                    if (gemA == null || gemB == null || gemC == null) continue;

                    if (gemA.GetValue().GetType() == gemB.GetValue().GetType()
                        && gemB.GetValue().GetType() == gemC.GetValue().GetType())
                    {
                        matches.Add(new Vector2Int(x, y));
                        matches.Add(new Vector2Int(x, y + 1));
                        matches.Add(new Vector2Int(x, y + 2));
                    }
                }
            }

            // No sound logic here, only return matches
            return new List<Vector2Int>(matches);
        }

        IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            var gridObjectA = grid.GetValue(gridPosA.x, gridPosA.y);
            var gridObjectB = grid.GetValue(gridPosB.x, gridPosB.y);

            gridObjectA.GetValue().transform
                .DOLocalMove(grid.GetWorldPositionCenter(gridPosB.x, gridPosB.y), 0.5f)
                .SetEase(ease);
            gridObjectB.GetValue().transform
                .DOLocalMove(grid.GetWorldPositionCenter(gridPosA.x, gridPosA.y), 0.5f)
                .SetEase(ease);

            grid.SetValue(gridPosA.x, gridPosA.y, gridObjectB);
            grid.SetValue(gridPosB.x, gridPosB.y, gridObjectA);

            yield return new WaitForSeconds(0.5f);
        }

        void InitializeGrid()
        {
            grid = GridSystem2D<GridObject<Gem>>.VerticalGrid(width, height, cellSize, originPosition, debug);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    CreateGem(x, y);
                }
            }
        }

        void CreateGem(int x, int y)
        {
            var gem = Instantiate(gemPrefab, grid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);
            gem.SetType(gemTypes[Random.Range(0, gemTypes.Length)]);
            var gridObject = new GridObject<Gem>(grid, x, y);
            gridObject.SetValue(gem);
            grid.SetValue(x, y, gridObject);
        }

        void DeselectGem()
        {
            grid.GetValue(selectedGem.x, selectedGem.y).GetValue().SetSpriteAsSelected(false);
            selectedGem = new Vector2Int(-1, -1);
        }
        void SelectGem(Vector2Int gridPos)
        {
            grid.GetValue(gridPos.x, gridPos.y).GetValue().SetSpriteAsSelected(true);
            selectedGem = gridPos;
        }

        bool IsEmptyPosition(Vector2Int gridPosition) => grid.GetValue(gridPosition.x, gridPosition.y) == null;

        bool IsValidPosition(Vector2 gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
        }

        // Paul Code
        public List<Vector2Int> AllMatches;
        public void CountDestroyedGems(List<Vector2Int> matches)
        {
            AllMatches.AddRange(matches);
        }
    }
}
