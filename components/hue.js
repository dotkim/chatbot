require('dotenv').config();
const HueApi = require('node-hue-api').HueApi;


const host = process.env.HUEHOST;
const username = process.env.HUEUSER;
const api = new HueApi(host, username);

class HueController {
  async getState() {
    let data = await api.getLights();
    // build the response array
    let reply = '';
    data.lights.forEach((light) => {
      let state;
      if (light.state.on) {
        state = ':large_blue_circle:';
      }
      else {
        state = ':red_circle:';
      }
      reply += '`' + light.id + '`:\t' + state + '\t' + light.name + '\n';
    });
    return reply;
  }

  setState(lightID, state) {
    try {
      if (state === 'on') {
        state = true;
      }
      else if (state === 'off') {
        state = false;
      }
      api.setLightState(lightID, { on: state });
    }
    catch (error) {
      console.error(error);
    }
  };
}

module.exports = HueController;