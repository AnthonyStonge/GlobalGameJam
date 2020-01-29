using UnityEngine;
using System.Collections.Generic;

namespace PixelCrushers.SceneStreamer
{
	[AddComponentMenu("Scene Streamer/Scene Edge")]
	public class SceneEdge : MonoBehaviour
	{
		
		/// The current scene root.
		public GameObject currentSceneRoot;
		
		/// The name of the next scene on the other side of the edge.
		public string nextSceneName;

		public List<string> acceptedTags = new List<string>() { "Player" };
		
		/// When the player enters this edge (for example coming in from a neighbor),
		/// makes sure to set the current scene to this edge's scene.
		public void OnTriggerEnter(Collider other) 
		{
			CheckEdge(other.tag);
		}
		
		/// When the player enters this edge (for example coming in from a neighbor),
		/// makes sure to set the current scene to this edge's scene.
		public void OnTriggerEnter2D(Collider2D other)
		{

			CheckEdge(other.tag);
		}

		private void CheckEdge(string otherTag)
		{
			if (acceptedTags == null || acceptedTags.Count == 0 || acceptedTags.Contains(otherTag))
			{
				SetCurrentScene();
			}
		}

		private void SetCurrentScene()
		{
			if (currentSceneRoot) SceneController.SetCurrentScene(currentSceneRoot.name);
		}

	}

}