let getPageContents = () => {
    let elementsContent = [], words = [];
    Array.from(document.getElementsByTagName("p")).forEach((element) => {
        let content = String(element.innerText)
        let style = window.getComputedStyle(element);
        if (style.display !== 'none' && element.offsetHeight > 0) elementsContent.push(content);
    });
    
    elementsContent.forEach((element) => {
        words = words.concat(element.toLowerCase().replace(/\n/g, ' ').replace(/[^\w\s]/g, '').split(' '));
    });
    return words;
}

let res = getPageContents();
chrome.runtime.sendMessage({pageContents: res, type: "pageScan", pageName: document.title}, null);

let resData = [];
let elements = 0; 
let elementsScanned = 0;


chrome.extension.onMessage.addListener((msg, sender, sendResponse) => {

    let elementsContent = [], words = [];
    
    console.log(msg);
   
    if (msg.type == "COLOR-PH") {
        // style parent paragraph with border and background color according
        // to sentiment value
        let elt = window.getSelection().anchorNode.parentElement;
        // if the selection was a paragraph click, 
        if (elt.tagName == undefined) {
            // get all the scanned elements 
            Array.from(document.getElementsByClassName("sentemo-scanned")).forEach((element) => {
                // find the scanned element that matches the one we want to color 
                if (element.innerText == msg.content) {
                    elt = element;
                }
            });
        }
        // fix for bug where parentElement isn't the paragraph above
        if (elt.tagName.toLowerCase() != "p") {
            elt = elt.parentElement;
        }
        // add class to signify text has been scanned
        elt.classList.add("sentemo-scanned");
        // style with color  
        console.log(msg.data.sentimentColor)
        elt.style.border = "1px solid " + "hsl(" + msg.data.sentimentColor + ", 100%, 50%)";
        elt.style.borderRadius = "4px";
        elt.style.backgroundColor = "hsla(" + msg.data.sentimentColor + ", 100%, 50%, 0.3)";
        // add tooltip with sentiment values 
        elt.classList.add("sentemo-tooltip");
        let spanWithResults = document.createElement("SPAN");
        spanWithResults.classList.add("sentemo-tooltiptext");
        spanWithResults.innerHTML = "Sentiment: " + msg.data.descriptorSentiment + "</br>" +
        "Pleasantness: " + msg.data.descriptorPleasantness + "</br>" +
        "Attention Value: " + msg.data.descriptorAttention;
        
        // add tooltip to selected paragraph
        elt.appendChild(spanWithResults);
        // clear the selection
        if (window.getSelection) window.getSelection().removeAllRanges();
        else if (document.selection) document.selection.empty();
    } 
    else if (msg.type == "COLOR-WD-PH") { 
        // color individual recognized words based off their polarity values 
        let element, content, oldContent;

         
            Array.from(document.getElementsByTagName("p")).forEach((elt) => {
                // if the selection matches the paragraph, 
                if (window.getSelection() == elt.innerText){
                    element = elt;
                    content = elt.innerText;
                } 
            });
        
        let position = 0;
        if (element != null) {
            let contentSplit = [], wordsInCombination = 0;
            // style of element 
            let style = window.getComputedStyle(element);
            // ensure the element is visible and not and advertisement 
            if (style.display !== 'none' && element.offsetHeight > 0) {
                // split into individual words
                contentSplit = contentSplit.concat(content.replace(/\n/g, ' ').split(' '));

                // loop through split words 
                contentSplit.forEach((word, i) => {
                    // if we don't need to skip a word 
                    if (wordsInCombination <= 0){
                        // if the word was matched in the scanned data 
                        if (msg.data.wordsOrdered[position] !== null) { 
                            // get number of words in combination- words have been joined with '_'
                            //wordsInCombination = (msg.data.wordsOrdered[position].word.match(/_/g)||[]).length;
                            wordsInCombination=2;
                            // if there are more than one words matched in a combination,
                            // don't do anything on the next few loops; decrement below
                            // otherwise, continue looping 
                            let fullString = word;
                            // join the next matched words together 
                            for (let j = i + 1; j <= i + wordsInCombination; j++) {
                                fullString += (" " + contentSplit[j]);
                                contentSplit[j] = "";
                            }
                            // add a span element with style and title 
                            contentSplit[i] = "<span style=\"background-color: " + 
                                                "hsla(" + Math.floor(Math.random() * 230) + 
                                               // "hsla(" + msg.data.wordsOrdered[position].color + 
                                                ", 100%, 50%, 0.3); border-radius: 5px;\" title=\"Matched: \'" + 
                                                //msg.data.wordsOrdered[position].word + 
                                                "shako" + 
                                                "\'&#13;Sentiment: " + 77 + 
                                                "\">" + fullString + "</span>";
                        } 
                        // if the word wasn't matched, leave it alone
                    } else {
                        // we needed to skip a word, decrement
                        wordsInCombination--;
                    }
                    // move to next scanned word
                    position++;
                });
                // join the words together with spaces 
                contentSplit = contentSplit.join(' ');
                // replace the paragraph with its OG content 
                element.innerHTML = contentSplit;
                let newSpan = document.createElement("SPAN");
                // add the sentiment data back to the paragraph 
                newSpan.innerHTML = oldContent;
                newSpan.classList.add("sentemo-tooltiptext");
                element.appendChild(newSpan);
            }
        }
    } 
    else if (msg.type == "GET-SELECTION") {
        // get the selected text on the page
        sendResponse({selection: window.getSelection().toString()});
    }
});