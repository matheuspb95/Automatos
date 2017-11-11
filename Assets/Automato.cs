using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automato : MonoBehaviour {
	public Texture2D texture;
	public int width, height;
	public int[,] pixels;

	public List<Color> color;

	[Range(0,8)]
	public int Alive, Permanent;
	// Use this for initialization
	void Start () {
		texture = new Texture2D(width, height);
		texture.filterMode = FilterMode.Point;
		GetComponent<Renderer>().material.mainTexture = texture;
		pixels = new int[width, height];
		Reset();
		StartCoroutine(UpdateAutomato());
	}

	public void AllBlack(){
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				pixels[i,j] = 0;
			}
		}
		UpdateTexture();
	}

	public void Reset(){
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				pixels[i,j] = Mathf.RoundToInt(Random.value);
			}
		}
		UpdateTexture();
	}

	void UpdateTexture(){
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				texture.SetPixel(i, j, color[pixels[i,j]]);
			}
		}
		texture.Apply();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			RaycastHit hit;
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
				return;

			Renderer rend = hit.transform.GetComponent<Renderer>();
			MeshCollider meshCollider = hit.collider as MeshCollider;

			if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
				return;

			Vector2 pixelUV = hit.textureCoord;
			int posx = (int)(pixelUV.x * width);
			int posy = (int)(pixelUV.y * height);
			
			pixels[posx, posy] = 1;

			UpdateTexture();
		}
	}

	IEnumerator UpdateAutomato(){
		while(true){
			yield return new WaitForSeconds(0.1f);
			int[,] buffer = new int[width, height];
			int result = 0;
			for(int i = 0; i < width; i++){
				for(int j = 0; j < height; j++){
					try { result += pixels[i-1,j-1]; } catch {}
					try { result += pixels[i  ,j-1]; } catch {}
					try { result += pixels[i+1,j-1]; } catch {}
					try { result += pixels[i-1,j  ]; } catch {}
					try { result += pixels[i  ,j  ]; } catch {}
					try { result += pixels[i+1,j  ]; } catch {}
					try { result += pixels[i-1,j+1]; } catch {}
					try { result += pixels[i  ,j+1]; } catch {}
					try { result += pixels[i+1,j+1]; } catch {}
					if(result < Permanent)
						buffer[i,j] = 0;
					if(result > Alive)
						buffer[i,j] = 0;
					if(result == Alive)
						buffer[i,j] = 1;
					if(result == Permanent)
						buffer[i,j] = pixels[i,j];
					result = 0;
				}
			}
			pixels = buffer;
			UpdateTexture();
		}
	}


}
