using System.Collections;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    private PlayerInputSystem inputs;
    private RectTransform rTrans;

    [SerializeField] float moveSpeed;
    [SerializeField] float endTime;

    private void Awake()
    {
        inputs = new PlayerInputSystem();
        rTrans = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine("EndGame");
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
    {
        if (inputs.Basic.End.IsPressed()) Application.Quit();
        rTrans.position = Vector3.Lerp(rTrans.position, new Vector3(450, 254, 0), Time.deltaTime * moveSpeed);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(endTime);
        Application.Quit();
        Debug.Log("Game Complete.");
    }
}
