using UnityEngine;
using System.Collections;

public class SeedPlanter : MonoBehaviour {

	// Use this for initialization
	public GameObject aura;
	public Transform planterAura;
	public SpriteRenderer auraRenderer;
	public float radius = 5f;
	private Vector2 maxAuraScale;
	private bool planting = false;
	private float auraGrowTime = 0.5f;
	private float auraShrinkTime = 0.3f;
	private Vector2 startingRendererSize;

	void Start () {
		aura = Resources.Load ("Prefabs/Aura") as GameObject;
		planterAura = (Instantiate(aura) as GameObject).transform;
		auraRenderer = planterAura.GetComponent<SpriteRenderer> ();
		auraRenderer.color = new Color(1f,1f,1f,0.3f);
		auraRenderer.enabled = false;
		startingRendererSize = auraRenderer.bounds.size;
		maxAuraScale = new Vector2 ( 2f *radius / startingRendererSize.x, 2f *radius / startingRendererSize.y);
	}

	// Update is called once per frame
	float t = 0f;
	void Update () {


		if (Input.GetKeyDown(KeyCode.Z)) {
			t = 0f;
			planting = true;
		}
		if (Input.GetKeyUp(KeyCode.Z)) {
			t = 0f;
			planting = false;
			float currentRadius = planterAura.localScale.x * startingRendererSize.x / 2f;
			Collider2D[] Seeds = Physics2D.OverlapCircleAll (transform.position, currentRadius);
			foreach (Collider2D CSeed in Seeds) {
				MonoBehaviour[] list = CSeed.gameObject.GetComponents<MonoBehaviour>();
				foreach(MonoBehaviour mb in list){
					if(mb is ISeed){
						ISeed seed= mb as ISeed;
						seed.shine();
						seed.grow();
					}
				}
			}
		}
		if (planting) {
			auraRenderer.enabled = true;
			planterAura.localScale = Vector2.Lerp(new Vector2(0.1f,0.1f), maxAuraScale, t / auraGrowTime);
			t += Time.deltaTime;
		} else {
				planterAura.localScale = Vector2.Lerp(planterAura.localScale, new Vector2(0.1f, 0.1f), t / auraShrinkTime);
				t += Time.deltaTime;
			if (t/auraShrinkTime >= 1f) {
				auraRenderer.enabled = false;
			}
		}
		planterAura.position = transform.position;
	}

}
