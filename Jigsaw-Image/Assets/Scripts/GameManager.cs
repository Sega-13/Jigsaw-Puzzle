using System.Collections;
using System.Collections.Generic;
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
    private List<Transform> pieces;
    private Vector2Int dimensions;
    private Texture2D jigsawTexture;
    private float width, height;



    void Start()
    {
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
        DifficultyLevelScreen.gameObject.SetActive(false);

        int difficulty = DifficultyLevelButton.instance.GetDifficulty();
        Texture2D currentImage = GetCurrentJiswaImage();
        dimensions = GetJigsawDimension(currentImage,difficulty);
        CreateJigsawPieces(currentImage);
        Scatter();
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
        // calculate visible orthograhic size of screen.
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);

        float pieceWidth = width*GameHolder.localScale.x;
        float pieceHeight = height*GameHolder.localScale.y;

        orthoWidth -= pieceWidth;
        orthoHeight -= pieceHeight;
        foreach(Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, -1);
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
