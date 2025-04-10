// Увери се, че signalr.min.js е зареден преди този скрипт!

// Създаваме отделна връзка за NotificationHub
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Слушаме за събитие "ReceiveChatNotification" от NotificationHub
notificationConnection.on("ReceiveChatNotification", (fromUserId, message, createdAt) => {
    console.log("Received Chat Notification:", fromUserId, message, createdAt);
    // Показваме toast уведомление – прехвърляме линк към чата с конкретния потребител
    showToastNotification(message, `/Chat/With/${fromUserId}`);
});

// Допълнителен обработчик, ако получиш и други известия
notificationConnection.on("ReceiveNotification", (message) => {
    showToast(message);
});

// Започваме връзката
notificationConnection.start()
    .then(() => console.log("NotificationHub connected"))
    .catch(err => console.error("NotificationHub error: ", err));

// Функция за показване на уведомление (toast) в долния десен ъгъл
function showToast(message) {
    const container = document.getElementById("toastContainer");
    if (!container) return;

    const toast = document.createElement("div");
    toast.classList.add("toast");
    toast.innerText = message;

    container.appendChild(toast);

    setTimeout(() => { toast.remove(); }, 5000);
}

function showToastNotification(message, url) {
    const container = document.getElementById("toastContainer");
    if (!container) return;

    const toast = document.createElement("div");
    toast.classList.add("toast");
    // Вътре имаме линк – когато кликнеш, отиваш в чата с конкретния потребител
    toast.innerHTML = `<a href="${url}" style="color:white; text-decoration:none;">${message}</a>`;

    container.appendChild(toast);

    setTimeout(() => { toast.remove(); }, 6000);
}
