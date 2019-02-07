import { observable } from 'aurelia-framework';
import './styles/app.css';

export class App {
  @observable color = 'blue';

  colorChanged(newValue, oldValue) {
    // this will fire whenever the 'color' property changes
    console.log({ 'newValue': newValue, 'oldValue': oldValue });
  }

  message = 'Hello World!';
}
