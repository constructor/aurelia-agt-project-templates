import { observable } from 'aurelia-framework';
import './styles/app.css';

export class App {
  @observable message = 'Hello World!';

  colorChanged(newValue, oldValue) {
    // this will fire whenever the 'message' property changes
    console.log({ 'newValue': newValue, 'oldValue': oldValue });
  }

}
