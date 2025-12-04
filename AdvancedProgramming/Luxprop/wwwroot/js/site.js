window.scrollToBottom = (element) => {
    try {
        if (element) element.scrollTop = element.scrollHeight;
    } catch (e) { }
};

window.downloadFile = (fileName, contentType, base64Data) => {
    const link = document.createElement('a');
    link.href = "data:" + contentType + ";base64," + base64Data;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
