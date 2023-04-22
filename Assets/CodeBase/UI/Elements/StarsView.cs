using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class StarsView : MonoBehaviour
    {
        [SerializeField] private int _maxStars;
        [SerializeField] private GameObject[] _stars;

        public void EnableStars(int starsCount)
        {
            if (starsCount > _maxStars)
            {
                starsCount = _maxStars;
            }

            for (int i = 0; i < starsCount; i++)
            {
                _stars[i].SetActive(true);
            }
        }
    }
}