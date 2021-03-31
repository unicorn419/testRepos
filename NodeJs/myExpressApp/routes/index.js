var express = require('express');
var router = express.Router();
var querystring = require('querystring')

/* GET home page. */
router.get('/', function(req, res, next) {
  // let t= querystring.parse(req.query);
  res.render('index.html', { title: 'Express'+ req.query['test'] });
});

module.exports = router;
