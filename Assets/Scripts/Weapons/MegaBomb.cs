using UnityEngine;

public class MegaBomb : MonoBehaviour
{
    #region Components
    [SerializeField] private ParticleSystem _bombFX;
    #endregion
    /// <summary>
    /// TO-DO:
    /// ���������� �������� ��������� ������������ ������������� ����� (private bool _isPlaying = false) ��� �������������� �������������� ������� ����
    /// </summary>
    #region Fields
    [Header("�������� ���������")]
    [Range(1f, 10f)]
    [SerializeField] private float _radius = 2f;
    [SerializeField] private float _damage = 5f;
    #endregion

    public void DeployBomb()
    {
        Debug.Log("Bomb deployed");
        _bombFX.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (var coll in colliders)
        {
            HealthSystem health = coll.GetComponent<HealthSystem>();

            if (health != null && coll.CompareTag("Enemy"))
            {
                health.TakeDamage(_damage, coll);
                Debug.Log(coll.name + " received bomb damage");
            }
        }
    }

    private void Start()
    {
        var partMain = _bombFX.main;
        partMain.startSize = _radius * partMain.startSize.constant;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DeployBomb();
        }
    }
}