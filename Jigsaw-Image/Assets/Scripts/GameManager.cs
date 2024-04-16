using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform startScreenPanel;
    [SerializeField] private Transform imageContent;
    [SerializeField] private Image levelSelectPrefab;
    [SerializeField] private Transform DifficultyScreen;
    [SerializeField] private Image myImage;
    [SerializeField] private Transform DifficultyLevelScreen;
    [SerializeField] private Transform GameHolder;
    [SerializeField] private Transform PiecePrefab;
    [SerializeField] private Transform VScroll;
    [SerializeField] private Transform HScrollContent;
    [SerializeField] private Transform GameCompleteScreen;
    [SerializeField] private Transform GamePlayPanel;
    [SerializeField] private Transform PausePanel;
    [SerializeField] private TextMeshProUGUI timeTaken;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI CoinEarnedText;
    private List<Transform> pieces;
    private Vector2Int dimensions;
    private Texture2D jigsawTexture;
    private float width, height;
    private Transform dragingPiece;
    private Vector3 offset;
    private int piecesCorrect;
    private bool isGamePaused;
    private int coins;
    private  int coinsEarned;

    enum GameLevel{
        Easy = 4,
        Medium = 6,
        Difficult = 8
    }
    void Start()
    {
        coinsEarned = PlayerPrefs.GetInt("CoinsEarned", coinsEarned);
        InstantiatingImages();
       
    }

    void InstantiatingImages()
    {
        
        foreach (Texture2D texture in imageTextures)
        {
            Image image = Instantiate(levelSelectPrefab, imageContent);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            image.GetComponent<Button>().onClick.AddListener(delegate {StartGame(texture); });
        }
    }

    public void StartGame(Texture2D jigsawTexture)
    {
        //Hide UI
        startScreenPanel.gameObject.SetActive(false);
        DifficultyScreen.gameObject.SetActive(true);
        pieces = new List<Transform>();
        myImage.sprite = Sprite.Create(jigsawTexture, new Rect(0, 0, jigsawTexture.width, jigsawTexture.height), Vector2.zero);
        SetCurrentJigsawImage(jigsawTexture);
       


    }
    void SetCurrentJigsawImage(Texture2D jigsawTexture)
    {
        this.jigsawTexture = jigsawTexture;
        
    }
    Texture2D GetCurrentJiswaImage()
    {
        return jigsawTexture;
    }
    public void GamePlay()
    {
        if (!isGamePaused)
        {
            DifficultyLevelScreen.gameObject.SetActive(false);
            GamePlayPanel.gameObject.SetActive(true);
            int difficulty = DifficultyLevelButton.instance.GetDifficulty();
            SetCoinValue(difficulty);
            Texture2D currentImage = GetCurrentJiswaImage();
            dimensions = GetJigsawDimension(currentImage, difficulty);
            TimerController.instance.BeginTimer();
            CreateJigsawPieces(currentImage);
            Scatter();
            UpdateBorder();
            piecesCorrect = 0;
        }
        
    }
    void SetCoinValue(int difficulty)
    {
        switch(difficulty)
        {
            case (int)GameLevel.Easy:
                coins = 10;
                break;
            case (int)GameLevel.Medium:
                coins = 20;
                break;
            case (int)GameLevel.Difficult: 
                coins = 50;
                break;
        }
    }
    public void ShowPause()
    {
        PausePanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    Vector2Int GetJigsawDimension(Texture2D currentImage,int difficulty)
    {
        Vector2Int dimensions =  Vector2Int.zero;
        if(currentImage.width < currentImage.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty*currentImage.height)/ currentImage.width;
        }
        else
        {
            dimensions.x = (difficulty * currentImage.height) / currentImage.width;
            dimensions.y = difficulty;
        }
        return dimensions;
    }

    void CreateJigsawPieces(Texture2D currentImage)
    {
        // calculate piece size based on dimensions
        height = 1f / dimensions.y;
        float aspectRatio = (float) currentImage.width / currentImage.height;
        width = aspectRatio / dimensions.x;

        for(int row = 0; row< dimensions.y; row++)
        {
            for(int col = 0; col< dimensions.x; col++)
            {
                Transform piece = Instantiate(PiecePrefab, GameHolder);
                piece.localPosition = new Vector3((-width*dimensions.x/2)+(width*col)+(width/2),
                    (-height*dimensions.y/2)+(height*row)+(height/2),-1);
                piece.localScale = new Vector3(width, height, 1f);
                
                pieces.Add(piece);
                float width1 = 1f/ dimensions.x;
                float height1 = 1f/ dimensions.y;
                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1*col, height1*row);
                uv[1] = new Vector2(width1 * (col+1), height1 * row); 
                uv[2] = new Vector2(width1 * col, height1 * (row+1)); 
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1)); 
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;
                 piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", currentImage);
            }
        }
    }
    private void Scatter()
    {
      //  HScroll.gameObject.SetActive(true);
        // calculate visible orthograhic size of screen.
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);

        float pieceWidth = width*GameHolder.localScale.x;
        float pieceHeight = height*GameHolder.localScale.y;

        orthoWidth -= pieceWidth;
        orthoHeight -= pieceHeight;
       //   float x = -orthoWidth;
        /*foreach (Transform piece in pieces)
        {

            piece.position = new Vector3(x, -4, -1);
            x += pieceWidth;

        }
*/
        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);

            float y = Random.Range(-orthoHeight, orthoHeight);

            piece.position = new Vector3(x, y, -1);
            Debug.Log("Position " + piece.position);
        }
    }
    void UpdateBorder()
    {
        LineRenderer lineRenderer = GameHolder.GetComponent<LineRenderer>();
        float halfWidth = (width*dimensions.x)/2f;
        float halfHeight = (height * dimensions.y) / 2f;

        float borderZ = 0;
        lineRenderer.SetPosition(0,new Vector3(-halfWidth,halfHeight,borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = true;
    }
   
    void Update()
    {
        CoinEarnedText.text = coinsEarned.ToString();
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit =  Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
            if (hit)
            {
                dragingPiece = hit.transform;
                offset = dragingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
            }
        
        }
        if(dragingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndDisableIfCorrect();
            dragingPiece.position += Vector3.forward;
            dragingPiece = null;
        }

        if (dragingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition += offset;
            dragingPiece.position = newPosition;
        }
        
    }
    void SnapAndDisableIfCorrect()
    {
        int pieceIndex = pieces.IndexOf(dragingPiece);
        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;

        Vector2 targetPosition = new((-width*dimensions.x/2)+(width*col)+(width/2),
            (-height*dimensions.y/2)+(height*row)+(height/2));
        if (Vector2.Distance(dragingPiece.localPosition, targetPosition) < (width / 2))
        {
            dragingPiece.localPosition = targetPosition;
            dragingPiece.GetComponent<BoxCollider2D>().enabled = false;
            piecesCorrect++;
            if(piecesCorrect == pieces.Count)
            {
                GamePlayPanel.gameObject.SetActive(false);
                TimerController.instance.EndTimer();
                GameCompleteScreen.gameObject.SetActive(true);
                coinsEarned += coins;
                coinText.text = coins.ToString();
                timeTaken.text = TimerController.instance.timerCounter.text;
                PlayerPrefs.SetInt("CoinsEarned",coinsEarned);
                PlayerPrefs.Save();
            }
        }
    }
    public void RestartGame()
    {
        // Destroy all puzzle pieces
        foreach(Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();
        GameHolder.GetComponent<LineRenderer>().enabled = false;
        GameCompleteScreen.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
        GamePlayPanel.gameObject.SetActive(false);
        VScroll.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
       PausePanel.gameObject.SetActive(false );
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public void PauseReset()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        foreach (Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();
        GameHolder.GetComponent<LineRenderer>().enabled = false;
        PausePanel.gameObject.SetActive(false);
        GamePlayPanel.gameObject.SetActive(false);
        DifficultyLevelScreen.gameObject.SetActive(true);
        
    }
}
