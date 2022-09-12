using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;

public class SliceManager : MonoBehaviour
{
    public static SliceManager instance;
    [SerializeField] private List<AskarLighsaber> _blade;
    public Sliceable[] _roads;
    [SerializeField] private Image _loadBar;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private CarController _car;

    private float _distance = 450f;
    private int _countBlade;
    private void Awake()
    {
        if (!instance) instance = this;
    }
    public void StartCut()
    {
        _blade[0].Init();
    }

    public async void RoadCollection(AskarLighsaber lighsaber)
    {
        if (lighsaber == _blade[_blade.Count - 1])
        {
            await Task.Delay(500);
            _roads = FindObjectsOfType<Sliceable>();
            DisableRoads();
            StartCoroutine(LoadScene());
            return;
        }
        _blade.Remove(lighsaber);
        StartCut();
    }
    private void DisableRoads()
    {
        for (int i = 0; i < _roads.Length - 1; i++)
        {
            _roads[i].gameObject.SetActive(false);
        }
    }
    IEnumerator LoadScene()
    {
        while (_loadBar.fillAmount >= 0.9f)
        {
            _loadBar.fillAmount = Mathf.Lerp(_loadBar.fillAmount, 1f, Time.deltaTime * 3f);
            yield return null;
        }
        _canvas.SetActive(false);
        _car.gameObject.SetActive(true);
    }

    public void GetClosestRoad(Transform player)
    {
        float distanceToClosestRoad = Mathf.Infinity;
        Sliceable road = _roads[0];
        foreach (var item in _roads)
        {
            if (!item.gameObject.activeSelf)
            {
                var distanceToRoad = (item.PosZ - player.position.z);
                if (distanceToRoad <= distanceToClosestRoad)
                {
                    distanceToClosestRoad = distanceToRoad;
                    road = item;
                }
            }
        }
        if ((road.PosZ - player.position.z) <= _distance) road.gameObject.SetActive(true);
    }
}
