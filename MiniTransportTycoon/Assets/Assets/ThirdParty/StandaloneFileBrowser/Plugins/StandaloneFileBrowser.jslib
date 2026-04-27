var StandaloneFileBrowserWebGLPlugin = {
    UploadFile: function (gameObjectNamePtr, methodNamePtr, filterPtr, multiselect) {
        var gameObjectName = UTF8ToString(gameObjectNamePtr);
        var methodName = UTF8ToString(methodNamePtr);
        var filter = UTF8ToString(filterPtr);

        var fileInput = document.getElementById(gameObjectName);
        if (fileInput) {
            document.body.removeChild(fileInput);
        }

        fileInput = document.createElement('input');
        fileInput.setAttribute('id', gameObjectName);
        fileInput.setAttribute('type', 'file');
        fileInput.setAttribute('style', 'display:none; visibility:hidden;');

        if (multiselect) {
            fileInput.setAttribute('multiple', '');
        }

        if (filter) {
            fileInput.setAttribute('accept', filter);
        }

        fileInput.onclick = function () {
            this.value = null;
        };

        fileInput.onchange = function (event) {
            var urls = [];
            for (var i = 0; i < event.target.files.length; i++) {
                urls.push(URL.createObjectURL(event.target.files[i]));
            }

            SendMessage(gameObjectName, methodName, urls.join());

            document.body.removeChild(fileInput);
        };

        document.body.appendChild(fileInput);

        document.onmouseup = function () {
            fileInput.click();
            document.onmouseup = null;
        };
    },

    DownloadFile: function (gameObjectNamePtr, methodNamePtr, filenamePtr, byteArray, byteArraySize) {
        var gameObjectName = UTF8ToString(gameObjectNamePtr);
        var methodName = UTF8ToString(methodNamePtr);
        var filename = UTF8ToString(filenamePtr);

        var bytes = new Uint8Array(byteArraySize);
        for (var i = 0; i < byteArraySize; i++) {
            bytes[i] = HEAPU8[byteArray + i];
        }

        var downloader = window.document.createElement('a');
        downloader.setAttribute('id', gameObjectName);
        downloader.href = window.URL.createObjectURL(new Blob([bytes], { type: 'application/octet-stream' }));
        downloader.download = filename;
        document.body.appendChild(downloader);

        document.onmouseup = function () {
            downloader.click();
            document.body.removeChild(downloader);
            document.onmouseup = null;

            SendMessage(gameObjectName, methodName);
        };
    }
};

mergeInto(LibraryManager.library, StandaloneFileBrowserWebGLPlugin);