using UnityEngine;

namespace Match3 {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Gem : MonoBehaviour {
        public GemType type;

        public void SetType(GemType type) {
            this.type = type;
            GetComponent<SpriteRenderer>().sprite = type.sprite;
        }

        public void SetSpriteAsSelected(bool isSelected)
        {
            if (isSelected)
            {
                GetComponent<SpriteRenderer>().sprite = type.selectedSprite;
            }
            else
            {
                Debug.Log("OFF OFF OFF");
                GetComponent<SpriteRenderer>().sprite = type.sprite;
            }
        }
        
        public GemType GetType() => type;
    }
}