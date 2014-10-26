using UnityEngine;
using System.Collections;

public static class WaveCreatorHelperFunctions {
	
	public static Texture2D CreateHeightmap () {
		
		// Assign the variable to the current terrain
		Terrain terrain = Terrain.activeTerrain;

		if (terrain != null) {
		
			// Assign the terrain heights to a variable
			float [,] heightmapValues = terrain.terrainData.GetHeights (0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);

			Texture2D heightmap = new Texture2D (512, 512);

			for (var x = 0; x < 512; x++) {
					for (var y = 0; y < 512; y++) {
		
							heightmap.SetPixel (x, y, Color.white * heightmapValues [y, x]);
					}
			}

			heightmap.Apply ();

			return heightmap;
		}

		return Resources.Load ("Clear Heightmap", typeof (Texture)) as Texture2D;
	}

	
	public static float GetTerrainMaxHeight () {
		
		// Assign the variable to the current terrain
		Terrain terrain = Terrain.activeTerrain;
		
		if (terrain != null)
			return terrain.terrainData.size.y;
		else
			return Mathf.Infinity;
	}


	public static Vector3 PerlinVector3 (float time) {
		
		Vector3 rotationVector = Vector3.zero; // Creaate new Vector3.
		
		for (int i = 0; i < 3; i++) {
			
			rotationVector [i] = (Mathf.PerlinNoise (time , i * 100) - 0.5f) * 2.0f; // Sets the x, y and z to different factors based off a perlin noise function, (the x inputed into the function is always t but the y inputed changes).
		}
		
		return rotationVector; // returns the vector
	}

	
	public static float WaveFunction (float x, float z, GameObject waterBody, bool fadeout) {
		
		// Variables used in the shader
		float [] ampArray = {1.0f,0.5f,0.3f,0.2f};			
		float [] steepnessArray = {1.0f,0.4f,0.6f,0.4f};
		float [] freqArray = {1.0f,0.5f,2.0f,2.5f};
		float [] velocityArray = {1.0f,0.5f,1.2f,1.2f};
		Vector2 [] dirArray = {Vector2.zero, Vector2.one, Vector2.right, Vector2.up};
		
		// Get properties
		float amp = waterBody.renderer.material.GetFloat ("_Amp");
		float freq = waterBody.renderer.material.GetFloat ("_Freq");
		float steepness = waterBody.renderer.material.GetFloat ("_Steepness");
		float dirx = waterBody.renderer.material.GetFloat ("_Dirx");
		float dirz = waterBody.renderer.material.GetFloat ("_Dirz");
		float dirType = waterBody.renderer.material.GetFloat ("_DirectionType");
		float velocity = waterBody.renderer.material.GetFloat ("_Velocity");
		float waveFadeout = waterBody.renderer.material.GetFloat ("_WaveFadeout");
		
		// Adjust Amplitude due to wave fadeout
		if (fadeout == true) {
			
			float view_distance = Vector3.Distance (new Vector3 (x, waterBody.transform.position.y, z), Camera.main.transform.position); 
			
			// The waves get smaller the further you are away from them
			amp = Mathf.Max( 0.0f, (amp - ((amp * view_distance) / waveFadeout)));
		}
		
		// Create the cumulative height variable
		float cumulativeHeight = 0.0f;
		
		if (amp > 0) {
			
			// Change dirArray [0]
			dirArray [0] = new Vector2 (dirx, dirz);
			
			// Localise Coordinates
			Vector2 localCoords = new Vector2 (x - waterBody.transform.position.x, z - waterBody.transform.position.z);
			
			// Loop for each wave
			for (int i = 0; i < 4; i++) {
				
				// The x and z coordinates of the vertex relative to the source if circular
				Vector2 directionRelativePosition = Vector2.Lerp (localCoords, localCoords - new Vector2 (dirx + i, dirz - i), dirType);
				
				// The direction of the waves
				Vector2 dir = Vector2.Lerp (new Vector2 (dirArray[i].x, dirArray[i].y), directionRelativePosition.normalized, dirType);
				
				// Uses the Gerstner wave equation, for each wave it finds the correct value from the arrays above
				cumulativeHeight += (amp * ampArray[i]) * Mathf.Pow ((Mathf.Sin ((freq * freqArray[i] * Vector2.Dot (dir, directionRelativePosition)) + ((Time.time / 20.0f) * velocity * velocityArray[i])) + 1) / 2, 1 + (steepness * steepnessArray[i]));
				// new_position.y += (_Amp * amp_array[i]) * pow ((sin ((_Freq * freq_array[i] * dot (dir, flat_pos)) + (_Time.x * _Velocity * velocity_array[i])) + 1) / 2, 1 + (_Steepness * steepness_array[i]));
			}
			
		}
		
		return cumulativeHeight;
	}
}