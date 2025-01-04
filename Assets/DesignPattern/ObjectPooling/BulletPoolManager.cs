using System;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    private ObjectPool<GameObject> bulletPool;
    public GameObject bulletPrefab;

    private void Awake()
    {
        bulletPool = new ObjectPool<GameObject>
            (
                createFunc: CreateBullet, // ��ü ����
                actionOnGet: OnGetBullet, // Ǯ���� ��ü ������ �� ����
                actionOnRelease: OnReleaseBullet, // Ǯ���� ��ü ��ȯ�� �� ����
                actionOnDestroy: OnDestroyBullet, // Ǯ�� ���� á�� �� ����
                defaultCapacity: 10, // �ʱ� Ǯ ũ��
                maxSize: 20  // �ִ� Ǯ ũ��
            );
    }

    private void OnDestroy()
    {
        if (bulletPool != null)
        {
            bulletPool.Clear();
        }
    }

    private GameObject CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab, transform);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = UnityEngine.Random.rotationUniform;
        bullet.AddComponent<Bullet>().SetPool(bulletPool);
        return bullet;
    }

    private void OnGetBullet(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = UnityEngine.Random.rotationUniform;
    }

    private void OnReleaseBullet(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyBullet(GameObject obj)
    {
        Destroy(obj);
    }

    public void FireBullet()
    {
        var bullet = bulletPool.Get();
        bullet.GetComponent<Bullet>().Init();
    }
}
