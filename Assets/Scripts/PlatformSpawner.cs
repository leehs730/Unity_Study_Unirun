using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수=> 충분하지 않다면 count 값을 늘리면 됨

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격(발판을 배치할 때마다 매번 랜덤하게 값이 변경)

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        // count 만큼의 공간을 가지는 새로운 발판 생성
        platforms = new GameObject[count];

        // count 만큼 루프하면서 발판 생성
        for (int i = 0; i < count; i++)
        {
            // platformPrefab을 원본으로 새 발판을 poolPosition 위치에 복제 생성
            // 생성된 발판을 platform 배열에 할당
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);  //platfromPrefab은 Platform 프리팹의 복제본 생성, 생성위치는 poolPosition, 회전은 Quaternion.identity
        }

        // 마지막 배치 시점 초기화
        lastSpawnTime = 0f;
        // 다음번 배치까지의 시간 간격을 0으로 초기화
        timeBetSpawn = 0f;
    }


    
    void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치(start() 메서드에서 만든 발판 게임 오브젝트를 돌아가며 사용하는 방식으로 발판 무한 배치를 구현)
        // 게임오버 상태에서는 동작하지 않음
        if (GameManager.instance.isGameover)    // 싱글턴 게임 매니저에 접근하여 게임오버 상태를 확인
        {
            return; // isGameover가 true인 경우 이곳에서 Update() 메서드가 매번 종료되어 발판 재배치가 완전 멈춤. isGameover가 False라면 Update() 처리가 여기서 종료되지 않고 계속 진행
        }

        // 마지막 배치 시점에서 timeBetSpawn 이상 시간이 흘렸다면 => 발판 스폰시 발판 중복을 방지하기 위함
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 기록된 마지막 배치 시점을 현재 시점으로 갱신
            lastSpawnTime = Time.time;

            // 다음 배치까지의 시간 간격을 timeBetSpwanMin, timeBetSpawnMax 사이에서 랜덤 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            // 배치할 위치의 높이를 yMin과 yMax 사이에서 랜덤 설정
            float yPos = Random.Range(yMin, yMax);

            // 사용할 현재 순번의 발판 게임 오브젝트를 비활성화하고 즉시 다시 활성화
            // 이때 발판의 Platform 컴포넌트의 OnEnable 메서드가 실행됨
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            // 현재 순번의 발판을 화면 오른쪽에 재배치
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);   //xPos는 고정, yPos는 랜덤
            // 순번 넘기기
            currentIndex++;
            
            // 마지막 순번에 도달했다면 순번을 리셋
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}