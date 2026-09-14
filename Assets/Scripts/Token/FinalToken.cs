using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalToken : Token
{
    [SerializeField] Material[] inactiveMaterials;
    [SerializeField] Material[] activeMaterials;
    [SerializeField] float colorShiftTime;

    [Header("Ending Parameters")]
    [SerializeField] float colorShiftTimeReduction;
    [SerializeField] float localScaleExpansion;
    [SerializeField] float endScaleRequirement;

    int materialIndex;
    float timer;

    private void Update()
    {
        if (timer > colorShiftTime)
        {
            if (collected)
            {
                colorShiftTime = colorShiftTime * colorShiftTimeReduction;
                transform.localScale = transform.localScale * localScaleExpansion;
                if (transform.localScale.x > endScaleRequirement) SceneManager.LoadScene("End");
            }

            timer = 0;
            materialIndex++;
            if (materialIndex >= inactiveMaterials.Length) materialIndex = 0;
            if (collected) rend.material = activeMaterials[materialIndex];
            else rend.material = inactiveMaterials[materialIndex];
        }
        else timer += Time.deltaTime;
    }
}
