
function focusOnInputField(input) {
    if (input) {
        let element = document.getElementById(input);
        if (element) {
            element.focus();
        }
    }
}
function scrollToBottomOfDiv(input) {
    if (input) {
        let element = document.getElementById(input);
        if (element) {
          element.scrollTop = element.scrollHeight;
        }
    }
}

function syncHeaderTitle() {
    let element = document.getElementById("headerPageTitle");
    if (element) {
        element.innerHTML = document.title;
    }
}
function setHeaderTitle(title) {
    if (title) {
        let element = document.getElementById("headerPageTitle");
        if (element) {
            element.innerHTML = title;
        }
    }
}

// https://docs.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-6.0
async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;

    if (fileName) {
        anchorElement.download = fileName;
    }

    anchorElement.click();
    anchorElement.remove();
}

window.clipboardCopy = {
    copyText: function (textToCopy) {
        // navigator clipboard api needs a secure context to work (https)
        if (navigator.clipboard && window.isSecureContext) {
            return navigator.clipboard.writeText(textToCopy);
        } else {
            // use a hidden text area out of viewport to copy the data
            let textArea = document.createElement("textarea");
            textArea.value = textToCopy;
            textArea.style.position = "fixed";
            textArea.style.left = "-999999px";
            textArea.style.top = "-999999px";
            document.body.appendChild(textArea);
            textArea.focus();
            textArea.select();
            return new Promise((res, rej) => {
                document.execCommand('copy') ? res() : rej();
                textArea.remove();
            });
        }
    }
}
