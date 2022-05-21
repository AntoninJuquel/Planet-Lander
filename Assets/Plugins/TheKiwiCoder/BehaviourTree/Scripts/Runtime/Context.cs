using UnityEngine;

namespace TheKiwiCoder
{
    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context
    {
        public GameObject gameObject;
        public Transform transform;
        public Rigidbody2D rb;

        public BoxCollider2D col;
        // Add other game specific systems here
        
        public static Context CreateFromGameObject(GameObject gameObject)
        {
            // Fetch all commonly used components
            var context = new Context
            {
                gameObject = gameObject,
                transform = gameObject.transform,
                rb = gameObject.GetComponent<Rigidbody2D>(),
                col = gameObject.GetComponent<BoxCollider2D>(),
            };

            // Add whatever else you need here...

            return context;
        }
    }
}