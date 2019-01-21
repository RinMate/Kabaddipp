const express     = require('express');
const app         = express();
const expressWs   = require('express-ws')(app);
const path = require('path')
const os = require('os')
const port = process.env.PORT || 3000

let connects = [];
let hmd_connect;
let hmd_width = -1;
let hmd_height = -1;
let wc_connect;

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
        if(!hmd_connect){
          hmd_connect.send(str);
        }
        if(!wc_connect){
          wc_connect.send(str);
        }
        /*connects.forEach(socket => {
          socket.send(str);
        });*/
        break;
      case "register":
        if(obj.device === 'hmd'){
          hmd_connect = ws;
          console.log('HMD set');
        }else if(obj.device === 'webcam'){
          wc_connect = ws;
          console.log('WebCam set');
          if(!hmd_connect){
            str = JSON.stringify({
              'type': 'register',
              'data': 'webcam'
            })
            hmd_connect.send(str);
          }
        }
        break;
      case "webcam":
        if(!hmd_connect){
          str = JSON.stringify({
            'type': 'webcam',
            'data': obj.data
          })
          hmd_connect.send(str);
        }
        break;
      case 'webcam_init':
        if(!wc_connect){
          str = JSON.stringify({
            'type': 'webcam_init',
            'width': obj.width,
            'height': obj.height
          })
          wc_connect.send(str);          
        }
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