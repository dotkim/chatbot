const wol = require('wol');

module.exports = async function(mac) {
  let result = await wol.wake(mac);
  return result
}