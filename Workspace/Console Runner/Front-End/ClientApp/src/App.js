import React, {Component} from 'react';
import {Layout} from './components/Layout';
import {Home} from './components/Home';
import {FetchData} from './components/FetchData';
import { Counter } from './components/Counter';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Redirect,
} from "react-router-dom";
import './custom.css';
import Login from './Login';
import Signup from './Signup';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Login />,
            <Signup />,
            <><Router>
                <Switch>

                    <Route path="/Login" component={Login} />


                    <Route path="/Signup" component={Signup} />

                    <Redirect to="/" />
                </Switch>
            </Router></>
            /*<Layout>
              <Route exact path='/' component={Home} />
              <Route path='/counter' component={Counter} />
              <Route path='/fetch-data' component={FetchData} />
            </Layout>*/
        );
    }
}
