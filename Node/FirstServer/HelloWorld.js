const http = require('http');
var url = require('url');
var fs = require('fs');

const hostname = '127.0.0.1';
const port = 31000;

const server = http.createServer((req, res) => {
  var url_p = url.parse(req.url,true);
  var id = url_p.query.id;


  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('Hello World ' + id + '\n');
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});