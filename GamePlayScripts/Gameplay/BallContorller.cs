using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallContorller : MonoBehaviour
{
    Camera mainCamera;
    
    private Vector3 mousePos;
    public Vector2 mousePos2D { get;  set; }
    public RaycastHit2D hit { get;  set; }
    public RaycastHit2D obstacleHit { get; private set; }
    public RaycastHit2D quitAimingHit { get; private set; }
    public float clickTimer {get;set;}
    public bool _canShoot {get; set;}

    public Material[] ballTrailMaterials { 
        get => GetMaterialValue(PlayerPrefs.GetInt("ChosenBallTrail", 0)).trailMaterialArray; 
        private set => SetMaterialValue(1, null); 
    }

    Vector2 startingTouchPosition;
    Vector2 ballDirection;

    static List<GameObject> pointsOfTrajectory;
    static List<GameObject> pointsOfTrajectoryInner;
    static List<SpriteRenderer> pointsOfTrajectorySpriteRenderer;
    static List<SpriteRenderer> pointsOfTrajectorySpriteRendererInner;
    static Color ballColor;
    static GameObject bally;
    static SpriteRenderer startingPointRenderer;
    static SpriteRenderer _strechIndicatorRenderer;

    static GameObject _indicatorBally;
    SpawnBall spawn;
    PlayerStats playerStats;
    BallMotor _ballMotor;

    GameObject _strechIndicatorStrecher;

    Rigidbody2D rb;
    GameObject ball;


    LayerMask mask;
    LayerMask obstacleLayerMask;
    LayerMask quitAimingLayerMask;
    
    float torque;
    bool isTorqueLeft;
    bool isTorqueRight;
    float holdTime = 0.15f;
    float holdTimeForTrajectory = 0.1f;
    float ballSize=5;
    public float force;
    public float minimumTorque = 20;
    public int numberOfStepsForTrajectory = 16;
    public int maximumDrag = 10;
    public static bool canYouStartAiming = false;

    public GameObject torqueRightSlider;
    public GameObject torqueLeftSlider;
    public GameObject trajectoryIndicator;
    public GameObject trajectoryIndicatorInner;
    public GameObject startingPoint;
    public GameObject startingPointColor;

    [SerializeField] PhysicsMaterial2D _ballMaterial;
    [SerializeField] Animator quitAimingAnimator;

    [SerializeField] private MaterialArrayC[] myArrayOfMaterialArray;

    public void SetMaterialValue(int index, MaterialArrayC subClass)
    {
        // Perform any validation checks here.
        myArrayOfMaterialArray[index] = subClass;
    }
    public MaterialArrayC GetMaterialValue(int index)
    {
        // Perform any validation checks here.
        return myArrayOfMaterialArray[index];
    }

    public void SpawnTheBall()
    {
        if(torque == 0)
        {
            torque = minimumTorque;
        }
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayTubeSound();

        spawn = startingPoint.GetComponent<SpawnBall>();
        spawn.SpawnTheBall(ballDirection, torque, force);
    }
    // Torque controlls
    #region

    public void TroqueLeftSlider()
    {
        Slider rightSlider = torqueRightSlider.GetComponent<Slider>();
        rightSlider.value = 0;
        Slider leftSlider = torqueLeftSlider.GetComponent<Slider>();
        float value = leftSlider.value;
        clickTimer = 0;
        torque = -1000 * value;

    }
    public void TroqueRightSlider()
    {
        Slider leftSlider = torqueLeftSlider.GetComponent<Slider>();
        leftSlider.value = 0;
        Slider rightSlider = torqueRightSlider.GetComponent<Slider>();
        float value= rightSlider.value;
        clickTimer = 0;
        torque = 1000 * value;
    }
    public void TorqueLeftButton()
    {
        isTorqueLeft = !isTorqueLeft;
        if (isTorqueLeft)
        {
            torque = 300;
            isTorqueRight = false;
        }
        else
        {
            torque = 10;
        }
    }
    public void TorqueRightButton()
    {
        
        isTorqueRight = !isTorqueRight;
        if (isTorqueRight)
        {
            torque = -300;
            isTorqueLeft = false;
        }
        else
        {
            torque = 10;
        }
    }
    #endregion
   
    public void ThrowBall()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");

        if (ball != null)
        {
            SpawnTheBall();
        }
    }
    void FixedUpdate()
    {

        if (canYouStartAiming)
        {
            if (Input.GetMouseButton(0))
            {
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos2D = new Vector2(mousePos.x, mousePos.y);
                MouseButtonHoldActions();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (clickTimer > holdTime && _canShoot && !quitAimingHit)
                {
                    SpawnTheBall();
                    ThingsToDoOnRelease();
                    canYouStartAiming = false;
                }
                else if(quitAimingHit || _canShoot)
                {
                    ThingsToDoOnRelease();
                }

                    quitAimingAnimator.SetTrigger("HideQuitAiming");

            }
        }
    }

    public void MouseButtonHoldActions()
    {

        if (Input.GetMouseButtonDown(0))
        {

        bally = SpawnBall.ball;
        rb = bally.GetComponent<Rigidbody2D>();
        _ballMotor = bally.GetComponent<BallMotor>();
        
        rb.mass = PlayerPrefs.GetFloat("BallMass", 10);
        ballSize = PlayerPrefs.GetFloat("BallSize", 5f);
        _ballMaterial.bounciness = PlayerPrefs.GetFloat("BouncinessUpgrade", 0.5f);
        bally.transform.localScale = new Vector3(ballSize, ballSize, 1);
            ballColor = bally.GetComponent<SpriteRenderer>().color;
            MouseButtonDownActions();
        }
        ballDirection = mousePos2D - startingTouchPosition;

        if (ballDirection.magnitude > maximumDrag)
        {
            float angleSine = ballDirection.y / ballDirection.magnitude;
            float angleCosine = ballDirection.x / ballDirection.magnitude;
            float yMax = 0;
            float xMax = 0;
            yMax = angleSine * maximumDrag;
            xMax = angleCosine * maximumDrag;
            ballDirection = new Vector2(xMax, yMax);
        }

        Vector2 negativeDirection = -ballDirection;
        // The number 17f is the number that adjusted the line of trajectory with the actual path
        Vector2 ballVelocity = negativeDirection*17f/ rb.mass;
        Vector2 ballVelocityInner = negativeDirection*17f / rb.mass;



        startingPoint.transform.rotation = Quaternion.LookRotation(Vector3.forward, negativeDirection);
        _indicatorBally.transform.rotation =Quaternion.LookRotation(Vector3.forward, negativeDirection);
        //Tracking the position of input (finger on phone, mouse on computer)
        quitAimingHit = Physics2D.Raycast(mousePos2D, transform.forward, 1f, quitAimingLayerMask);
        Vector2[] somePositions = TrajectoryPathController.Plot(rb, startingPoint.transform.position, ballVelocity, numberOfStepsForTrajectory);
        Vector2[] somePositionsInner = TrajectoryPathController.Plot(rb, startingPoint.transform.position, ballVelocityInner, numberOfStepsForTrajectory);

        if (clickTimer > holdTimeForTrajectory)
        {
            if (playerStats.currentNumberOfBalls > 0){

                _strechIndicatorStrecher.transform.localScale = new Vector2(ballSize/60,ballDirection.magnitude/10);

                for (int i = 0; i < numberOfStepsForTrajectory; i++)
                {
                    try
                    {
                        pointsOfTrajectory[i].SetActive(true);
                        pointsOfTrajectory[i].transform.localScale = bally.transform.localScale/(7+(i/1.5f));
                        pointsOfTrajectory[i].transform.position = somePositions[i];

                        pointsOfTrajectoryInner[i].SetActive(true);
                        pointsOfTrajectoryInner[i].transform.localScale = bally.transform.localScale / (10+(i/1.5f));
                        pointsOfTrajectoryInner[i].transform.position = somePositionsInner[i];
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Out of bounds error" + e.Message);
                    }
                }
                   
            }

        }

        clickTimer += Time.deltaTime;
    }

    public void MouseButtonDownActions()
    {
        ChangeColorOfPathIndicators();
        hit = Physics2D.Raycast(mousePos2D, transform.forward, 1f, mask);
        obstacleHit = Physics2D.Raycast(mousePos2D, transform.forward, 1f, obstacleLayerMask);
        if (hit && !obstacleHit)
        {
            startingTouchPosition = mousePos2D;
            startingPoint.transform.position = mousePos2D;
            
            /////////////////////////////////////////////////////////////////////////////////////////////////
            _indicatorBally.transform.position = startingPoint.transform.position;
            _indicatorBally.GetComponentInChildren<TrailRenderer>().Clear();

            //////////////////////////////////////////////////
            _canShoot = true;
            quitAimingAnimator.SetTrigger("ShowQuitAiming");
        }
    }


    public void ThingsToDoOnRelease()
    {
        _indicatorBally.transform.position = new Vector3(100, 100, 0);
        _indicatorBally.GetComponentInChildren<TrailRenderer>().Clear();
        _strechIndicatorStrecher.transform.localScale = new Vector2(0,0);
        _canShoot = false;
        clickTimer = 0;
        startingPoint.transform.position = new Vector3(100, 100, 0);
        foreach (GameObject point in pointsOfTrajectory)
        {
            point.SetActive(false);
        }
        foreach (GameObject point in pointsOfTrajectoryInner)
        {
            point.SetActive(false);
        }

    }

    /// <summary>
    /// Changes color of trajectory indicators to the chosen color
    /// </summary>
    public static void ChangeColorOfPathIndicators()
    {
        _strechIndicatorRenderer.color = ballColor;

        foreach (SpriteRenderer renderer in pointsOfTrajectorySpriteRenderer)
        {
            renderer.color = new Color(ballColor.r, ballColor.g, ballColor.b, 0.15f);
        }
        foreach (SpriteRenderer renderer in pointsOfTrajectorySpriteRendererInner)
        {
            renderer.color = new Color(ballColor.r, ballColor.g, ballColor.b, 0.15f);
        }

        startingPointRenderer.color = ballColor;
        ManageIndicatorBalls();
    }

    static void ManageIndicatorBalls(){
        bally = SpawnBall.ball;
            
        if(_indicatorBally.name != bally.name){
            _indicatorBally=bally;
            _indicatorBally.transform.position = new Vector3(100, 100, 0);
            _indicatorBally.GetComponent<Rigidbody2D>().simulated = false;
            _indicatorBally.GetComponentInChildren<TrailRenderer>().Clear();
            _indicatorBally = (GameObject)Instantiate(bally) as GameObject;
            _indicatorBally.tag = "Untagged";
            string indicatorBallName = _indicatorBally.name.Replace("(Clone)","");
            _indicatorBally.name = indicatorBallName;
        }
    }
    void Awake()
    {
        mask = LayerMask.GetMask("Water");
        obstacleLayerMask = LayerMask.GetMask("Obstacles","UI");
        quitAimingLayerMask = LayerMask.GetMask("QuitAiming");

        mainCamera = Camera.main;
        LevelScore.score = 0;

        
        ballDirection = Vector3.zero;
        _indicatorBally = new GameObject();
        startingPointRenderer = startingPointColor.GetComponent<SpriteRenderer>();

       
        pointsOfTrajectory = new List<GameObject>();
        pointsOfTrajectoryInner = new List<GameObject>();
        pointsOfTrajectorySpriteRenderer = new List<SpriteRenderer>();
        pointsOfTrajectorySpriteRendererInner = new List<SpriteRenderer>();
        playerStats = gameObject.GetComponent<PlayerStats>();

        if (bally != null)
            ballColor = bally.GetComponent<SpriteRenderer>().color;

        startingPointRenderer.color = ballColor;

        if (ballColor == Color.clear)
        {
            ballColor = Color.black;
        }

        startingPoint.transform.position = new Vector3(100, 100, 0);

        canYouStartAiming = false;
        StartCoroutine(DoThingsOneAtATime());
    }

    IEnumerator DoThingsOneAtATime()
    {
    
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < numberOfStepsForTrajectory; i++)
        {
            GameObject indicator = (GameObject)Instantiate(trajectoryIndicator);
            indicator.name = "Indicator" + i;
            indicator.SetActive(false);
            pointsOfTrajectory.Insert(i, indicator);
            SpriteRenderer indicatorSR = indicator.GetComponent<SpriteRenderer>();
            pointsOfTrajectorySpriteRenderer.Insert(i, indicatorSR);
            indicator.transform.position = new Vector3(100, 100, 0);
        }

        for (int i = 0; i < numberOfStepsForTrajectory; i++)
        {
            GameObject indicator = (GameObject)Instantiate(trajectoryIndicatorInner);
            indicator.name = "IndicatorInner" + i;
            indicator.SetActive(false);
            pointsOfTrajectoryInner.Insert(i, indicator);
            SpriteRenderer indicatorSR = indicator.GetComponent<SpriteRenderer>();
            pointsOfTrajectorySpriteRendererInner.Insert(i, indicatorSR);
            indicator.transform.position = new Vector3(100, 100, 0);
        }

        yield return new WaitForSeconds(0.5f);

        Transform [] startingPointGOs =  startingPoint.GetComponentsInChildren<Transform>();
        foreach(Transform go in startingPointGOs){
            if(go.name == "StretchIndicatorStrecher"){
                _strechIndicatorStrecher = go.gameObject;
                _strechIndicatorRenderer = _strechIndicatorStrecher.GetComponentInChildren<SpriteRenderer>();
            }
        }

        bally = SpawnBall.ball;
        rb = bally.GetComponent<Rigidbody2D>();
        ChangeColorOfPathIndicators();
        TrajectoryPathController.triggeredIndicators = new bool[numberOfStepsForTrajectory];


        yield return new WaitForSeconds(0.7f);
        canYouStartAiming = true;
    }
}


[Serializable]
public class MaterialArrayC
{
    public Material[] trailMaterialArray;
}