using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silly
{
    public class Bullet : MonoBehaviour
    {
        //public string poolItemName = "Bullet";
        public float speed = 10.0f;
        public float endTime = 2.0f;            // �Ѿ��� ������� �ð�
        public float lifeTime = 0f;             // �Ѿ��� ��Ÿ���� ����� �ð�
        public int damage = 1;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame

        void Update()
        {
            lifeTime += Time.deltaTime;
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (lifeTime >= endTime)
            {
                lifeTime = 0f;
                Destroy(this.gameObject);
                //ObjectPool.Instance.PushToPool(poolItemName, gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.CompareTag("Enemy"))
            //{
            //    Debug.Log("Enemy");

            //    Enemy enemy = other.GetComponent<Enemy>();
            //    enemy.SetHp(damage);
            //    Destroy(this.gameObject);

            //}
        }
    }
}