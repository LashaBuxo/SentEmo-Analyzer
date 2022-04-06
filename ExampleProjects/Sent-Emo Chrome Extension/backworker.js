let resSelection;
let selection;

chrome.runtime.onInstalled.addListener((details) => {
    if (details.reason == "install") {

    }
    let id = chrome.contextMenus.create({
        "title": "Sent-Emo Analyzer",
        "contexts": ["selection"],
        "id": "textsel"
    });
});

//Receive Action command from keyboard: Ctrl/Shift/S 
chrome.commands.onCommand.addListener((command) => {
    if (command == "calculate-score-command") {
        handleSelectedText();
    }
});
//Receive Action command from right click
chrome.contextMenus.onClicked.addListener((contexts, tab) => {
    handleSelectedText();
});

let handleSelectedText = () => {
    chrome.tabs.query({ active: true, currentWindow: true }, (tabs) => {
        chrome.tabs.sendMessage(tabs[0].id, { type: "GET-SELECTION" }, (response) => {
            let selection = response.selection;
            if (selection != null && selection != undefined) {

                let cleaned = selection.toLowerCase().replace(/\n/g, ' ').replace(/[^\w\s-]/g, '').split(' ');

                resSelection = getSentimentAverage(cleaned, (res) => {
                    chrome.extension.onConnect.addListener((port) => {
                        port.postMessage({ data: res, title: selection });
                    })
            
                    chrome.tabs.query({ active: true, currentWindow: true }, (tabs) => {
                        chrome.tabs.sendMessage(tabs[0].id, { type: "COLOR-PH", data: res, content: selection }, null);
                    });
            
            
                    chrome.tabs.query({ active: true, currentWindow: true }, (tabs) => {
                        chrome.tabs.sendMessage(tabs[0].id, { type: "COLOR-WD-PH", data: res, content: selection }, null);
                    });
            
                });
            }
        });
    });
};

 
let getSentimentAverage = (data, callback) => {
    let ordered = [];
    ordered.push(null);
    let res = {
        sentimentColor: 100,
        wordsOrdered: ordered,
        words: data
    }
    if (callback != null) callback(res); 
};
