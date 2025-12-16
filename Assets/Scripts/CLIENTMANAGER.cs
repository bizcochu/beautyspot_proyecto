using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientManager : MonoBehaviour
{
    [Header("Prefabs y Spawn")]
    public GameObject[] clientPrefabs;
    public Transform spawnPoint;

    [Header("Puntos de cola")]
    public Transform[] queuePoints;

    [Header("Estaciones de Trabajo")]
    public Transform cajaTarget;
    public Transform doorTarget;

    [Header("Economía & UI")]
    public float currentMoney = 500;
    public TextMeshProUGUI moneyDisplay;
    public Dialogue dialogueUI;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;
    private float spawnTimer = 0f;

    private Queue<Client> queue = new Queue<Client>();

    void Start()
    {
        UpdateMoneyUI();
        SpawnClient();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnClient();
            spawnTimer = 0f;
        }
    }

    public void SpawnClient()
    {
        if (clientPrefabs == null || clientPrefabs.Length == 0)
        {
            Debug.LogError("ClientManager: No hay clientPrefabs asignados.");
            return;
        }

        int index = Random.Range(0, clientPrefabs.Length);
        GameObject obj = Instantiate(clientPrefabs[index], spawnPoint.position, spawnPoint.rotation);

        Client client = obj.GetComponent<Client>();
        if (client == null) return;

        client.doorWaypoint = doorTarget;
        client.exitPoint = doorTarget;
        client.OnArrived += OnClientArrived;

        AddClientToQueue(client);
    }

    public void AddClientToQueue(Client client)
    {
        queue.Enqueue(client);
        UpdateQueuePositions();
    }

    private void UpdateQueuePositions()
    {
        Client[] clients = queue.ToArray();
        for (int i = 0; i < clients.Length; i++)
        {
            if (i == 0) clients[i].MoveTo(cajaTarget);
            else
            {
                int index = Mathf.Min(i - 1, queuePoints.Length - 1);
                clients[i].MoveTo(queuePoints[index]);
            }
        }
    }

    private void OnClientArrived(Client client)
    {
        if (client == null) return;

        Client primerCliente = queue.Count > 0 ? queue.Peek() : null;

        if (client == primerCliente && client.currentTarget == cajaTarget)
        {
            string clientRequest = client.GetRequestDialogue();

            dialogueUI.ShowServiceSelection(
                clientRequest,
                () => OnDialogAccepted(client, ServiceType.CorteDePelo),
                () => OnDialogAccepted(client, ServiceType.LavadoDePelo),
                () => OnDialogAccepted(client, ServiceType.HacerPermanente)
            );
            return;
        }

        if (client.currentTarget == doorTarget)
        {
            CobrarServicio(client.lastMachineUsed);
            LiberarMaquina(client.lastMachineUsed);

            client.OnArrived -= OnClientArrived;
            Destroy(client.gameObject);

            UpdateQueuePositions();
        }
    }

    private void OnDialogAccepted(Client client, ServiceType serviceType)
    {
        if (client == null) return;

        MachinePoint station = StationManager.Instance.GetAvailableStation(serviceType);

        if (station != null)
        {
            client.AssignMachineTarget(station.transform);

            if (queue.Count > 0 && queue.Peek() == client)
            {
                queue.Dequeue();
                UpdateQueuePositions();
            }
        }
        else
        {
            client.FinishAndLeave();

            if (queue.Count > 0 && queue.Peek() == client)
            {
                queue.Dequeue();
                UpdateQueuePositions();
            }
        }
    }

    public void CobrarServicio(Transform machineTransform)
    {
        if (machineTransform == null) return;
        MachinePoint mp = machineTransform.GetComponent<MachinePoint>();
        if (mp != null)
        {
            currentMoney += mp.servicePrice;
            UpdateMoneyUI();
        }
    }

    void UpdateMoneyUI()
    {
        if (moneyDisplay != null)
            moneyDisplay.text = currentMoney.ToString("C0");
    }

    public void LiberarMaquina(Transform tr)
    {
        if (tr == null) return;
        MachinePoint mp = tr.GetComponent<MachinePoint>();
        if (mp != null) mp.isOccupied = false;
    }
}
