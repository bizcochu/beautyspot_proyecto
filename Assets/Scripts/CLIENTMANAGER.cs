using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientManager : MonoBehaviour
{
    [Header("Prefabs y Spawn")]
    public GameObject clientPrefab;
    public Transform spawnPoint;

    [Header("Puntos de cola")]
    public Transform[] queuePoints;

    [Header("Estaciones de Trabajo")]
    public Transform cajaTarget;
    // Ya no necesitamos referencias directas únicas, usaremos el StationManager
    // public Transform corteTarget;
    // public Transform manosTarget;
    // public Transform piesTarget;
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
        GameObject obj = Instantiate(clientPrefab, spawnPoint.position, spawnPoint.rotation);
        Client client = obj.GetComponent<Client>();

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

    // --- MAIN LOGIC CHANGE HERE ---
    private void OnClientArrived(Client client)
    {
        if (client == null) return;

        Client primerCliente = queue.Count > 0 ? queue.Peek() : null;

        // 1. CLIENTE LLEGA A LA CAJA
        if (client == primerCliente && client.currentTarget == cajaTarget)
        {
            // Obtener el diálogo basado en lo que el cliente quiere
            string clientRequest = client.GetRequestDialogue();

            // Mostrar UI con 3 opciones
            dialogueUI.ShowServiceSelection(
                clientRequest,
                () => OnDialogAccepted(client, ServiceType.CorteDePelo),
                () => OnDialogAccepted(client, ServiceType.LavadoDePelo),
                () => OnDialogAccepted(client, ServiceType.HacerPermanente)
            );

            return;
        }

        // 2. CLIENTE SALE (LLEGA A PUERTA)
        if (client.currentTarget == doorTarget)
        {
            CobrarServicio(client.lastMachineUsed);
            LiberarMaquina(client.lastMachineUsed);

            client.OnArrived -= OnClientArrived;
            Destroy(client.gameObject);

            UpdateQueuePositions();
            return;
        }
    }

    private void OnDialogAccepted(Client client, ServiceType serviceType)
    {
        if (client == null) return;

        // Buscar una estación libre para ese servicio
        MachinePoint station = StationManager.Instance.GetAvailableStation(serviceType);

        if (station != null)
        {
            // Mover al cliente a la estación encontrada
            client.AssignMachineTarget(station.transform);

            // Sacar de la cola
            if (queue.Count > 0 && queue.Peek() == client)
            {
                queue.Dequeue();
                UpdateQueuePositions();
            }
        }
        else
        {
            Debug.Log("No hay estaciones disponibles para este servicio.");
            // Aquí tenemos que manejar el caso donde no hay estaciones disponibles
        }
    }

    // --- ECONOMÍA ---
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
        if (moneyDisplay != null) moneyDisplay.text = currentMoney.ToString("C0");
    }

    public void LiberarMaquina(Transform tr)
    {
        if (tr == null) return;
        MachinePoint mp = tr.GetComponent<MachinePoint>();
        if (mp != null) mp.isOccupied = false;
    }
}