using UnityEngine;
 
 namespace CodeBase.Logic
 {
     public class Spire : MonoBehaviour
     {
         private static Vector3 _position;
 
         public const float DistanceToCenter = 6f;
 
         public static Vector3 Position => _position;
 
         private void Awake()
         {
             if (_position == default)
             {
                 _position = transform.position;
             }
             else
             {
                 Destroy(gameObject);
                 throw new System.Exception($"An object with the Spire class is already on the scene.The object named: {name} is destroyed.");
             }
         }
     }
 
 }