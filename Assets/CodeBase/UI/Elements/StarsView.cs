using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class StarsView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _stars;

        public void EnableStars(int starsCount)
        {
            int maxStars = _stars.Length;

            if (starsCount > maxStars)
            {
                starsCount = maxStars;
            }

            for (int i = 0; i < maxStars; i++)
            {
                _stars[i].SetActive(i < starsCount);
            }
        }
    }
}