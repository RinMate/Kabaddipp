var HOST = location.origin.replace(/^http/, 'ws');
var ws = new WebSocket(HOST);

function sendSkill (str) {
  var data = JSON.parse(str);
  ws.send(JSON.stringify({
    'type': 'useSkill',
    'data': data
  }));
  console.log('useSkill', str);
}

ws.onmessage = function(msg) {
  console.log(msg);
}
