using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// Проецирует точку p на отрезок AB и возвращает нормализованный параметр [0..1].
        /// 0 = точка A, 1 = точка B.
        /// </summary>
        public static float Project01(Vector3 p, Vector3 A, Vector3 B)
        {
            Vector3 AB = B - A;
            float len2 = Vector3.SqrMagnitude(AB);
            if (len2 < 1e-6f) return 0f;
            return Mathf.Clamp01(Vector3.Dot(p - A, AB) / len2);
        }
    }
}