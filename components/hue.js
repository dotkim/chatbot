require('dotenv').config();
const HueApi = require('node-hue-api').HueApi;


const host = process.env.HUEHOST;
const username = process.env.HUEUSER;
const api = new HueApi(host, username);

class HueController {
  async getState() {
    let data = await api.getLights();
    // build the response array
    let arr = [];
    data.lights.forEach((light) => {
      arr.push(light.id, light.state.on);
    });
    return arr;
  }
  
  setState(lightID, state) {
    try {
      api.setLightState(lightID, { on: state});
    }
    catch (error) {
      console.error(error);
    }
  };
}

module.exports = HueController;