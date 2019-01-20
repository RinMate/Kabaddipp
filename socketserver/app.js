const express     = require('express');
const app         = express();
const expressWs   = require('express-ws')(app);
const path = require('path')
const os = require('os')
const port = process.env.PORT || 3000

let connects = [];

// view engine setup
app.set('views', path.join(__dirname, 'views'))
app.set('view engine', 'pug')

// assets
app.use(express.static(path.join(__dirname, 'assets')))

// routes
app.use('/', require('./routes/index'))
app.use('/log', require('./routes/log'))

app.ws('/', (ws, req) => {
  connects.push(ws);

  ws.on('message', message => {
    console.log('Received -', message);
    var obj = JSON.parse(message);
    if("type" in obj){
      switch (obj.type)
      {
      case "useSkill":
        str = JSON.stringify({
          'type': 'skill',
          'data': obj.data
        })
        connects.forEach(socket => {
          socket.send(str);
        });
        break;
      default:
        break;
      }
    }
  });

  ws.on('close', () => {
    connects = connects.filter(conn => {
      return (conn === ws) ? false : true;
    });
  });
});

app.listen(port, () => {
  const ifaces = os.networkInterfaces()
  let network = void 0

  Object.keys(ifaces).forEach(ifname => {
    ifaces[ifname].forEach(iface => {
      if ('IPv4' !== iface.family || iface.internal !== false) return

      network = iface.address
    })
  })

  console.log(`local: http://localhost:${port}`)
  if (network) console.log(`external: http://${network}:${port}`)
});