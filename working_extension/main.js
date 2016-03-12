var ws = null;
var connected = false;

var serverUrl = "ws://localhost:36000/extension";
var connectionStatus;
var sendMessage;

var open = function() {
  console.log("trying to connect");
  var url = serverUrl;
  ws = new WebSocket(url);
  ws.onopen = onOpen;
  ws.onclose = onClose;
  ws.onmessage = onMessage;
  ws.onerror = onError;
}

var close = function() {
  if (ws) {
    console.log('CLOSING ...');
    ws.close();
  }
  connected = false;

}



var onOpen = function() {
console.log('OPENED: ' + serverUrl);
connected = true;
};

var onClose = function() {
  console.log('CLOSED: ' + serverUrl);
  ws = null;
  setTimeout(open,3000);

};

var onMessage = function(event) {
var data = event.data;
onNativeMessage(data);
};


var onError = function(event) {
  console.log(event.data);
  setTimeout(open,3000);
} 

function sendMessage(strings) {
  ws.send(strings);
}

function sendRunningTabs(close) {
    var listOfTabs = [];
    chrome.tabs.query({}, function (tabs) {
        tabs.forEach(function (tab) {
            var tabData = {
                "active": tab.active,
                "index": tab.index,
                "url": tab.url,
                "windowId": tab.windowId
            }
            listOfTabs.push(tabData);
      if (close == 1)
        chrome.tabs.remove(tab.id);
        });
      var json = JSON.stringify(listOfTabs);
      console.log(json);
      sendMessage(json);
    });
}


function openRunningTabs(json) {
  var list = json;
    var len = list.length;
    var lastWindowIndex = -1;
  for (var i = 0; i < len; ++i) {
    var tab = list[i];
    if (lastWindowIndex == -1) {
      lastWindowIndex = tab.windowId;
      chrome.tabs.update({ "url": tab.url });
    } else if (lastWindowIndex != tab.windowId) {
      chrome.windows.create({ "focused": true });
      chrome.windows.update(chrome.windows.WINDOW_ID_CURRENT, { "state": "maximized" });
      lastWindowIndex = tab.windowId;
      chrome.tabs.update({ "url": tab.url });
    } else {
      var tab2 = {
        "active": tab.active,
        "index": tab.index,
        "url": tab.url,
      };
      chrome.tabs.create(tab2);
    }
  }
}

function onNativeMessage(message) {
  //appendMessage("Received message: <b>" + JSON.stringify(message) + "</b>");
  console.log("recvd msg");
  console.log(typeof message);
  console.log(message);

  var info = JSON.parse(message)
  // var info = message;
  var type = info.type
  if(type==3){
  	  console.log("3 executed*******");
    var data = info.data;
    sendMessage("Opening Tabs");
    openRunningTabs(data);

  }
  else if(info.type == 1){
  	console.log("1 executed*******");
    var result = sendRunningTabs(0);
  }
  else if(info.type == 2){
   	console.log("2 executed*******");
    close();
    var result = sendRunningTabs(1);
    
  }
  else{
   	console.log("nothing executed*******");
    sendMessage("Wrong command");
  }
}

// function onDisconnected() {
//   port = null;
//   console.log("disconnected,,");
// }

function connect() {
  close();
  open();
  console.log("connected");
}



connect();





