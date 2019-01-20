const express = require('express')
const router = express.Router()

router.get('/', (req, res, next) => {
  res.render('log', {
    title: 'log'
  })
})

module.exports = router
