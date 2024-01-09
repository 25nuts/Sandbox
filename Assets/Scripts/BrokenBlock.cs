using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBlock : MonoBehaviour
{
    public float Timer = 4f;
    public GameObject Attachment;
    public float Heat = 0f;
    public float HeatConductivity = 0.1f;
    public float MeltingPoint = 100f;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer attachmentSpriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attachmentSpriteRenderer = Attachment.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Timer += Random.value;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            Instantiate(EffectObjects.instance.Shockwave, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        Heat -= Heat * HeatConductivity * Time.deltaTime;

        spriteRenderer.material.SetFloat("Heat", Heat);
        spriteRenderer.material.SetFloat("MeltingPoint", MeltingPoint);
        attachmentSpriteRenderer.GetComponent<SpriteRenderer>().material.SetFloat("Heat", Heat);
        attachmentSpriteRenderer.GetComponent<SpriteRenderer>().material.SetFloat("MeltingPoint", MeltingPoint);
    }
}
