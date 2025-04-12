
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

notificationConnection.on("ReceiveChatNotification", (fromUserId, message, createdAt) => {
    console.log("Received Chat Notification:", fromUserId, message, createdAt);
    showToastNotification(message, `/Chat/With/${fromUserId}`);
});

notificationConnection.on("ReceiveNotification", (message) => {
    showToast(message);
});

notificationConnection.start()
    .then(() => console.log("NotificationHub connected"))
    .catch(err => console.error("NotificationHub error: ", err));

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
    toast.innerHTML = `<a href="${url}" style="color:white; text-decoration:none;">${message}</a>`;

    container.appendChild(toast);

    setTimeout(() => { toast.remove(); }, 6000);
}
