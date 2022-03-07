import './Login.css';
import React from 'react';

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {email: '', password: ''};
        this.handleChange = this.handleChange.bind(this);
        this.sendLogin = this.sendLogin.bind(this);
        //this.email = React.createRef();
        //this.password = React.createRef();
    }

    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
    async sendLogin(e) {
        e.preventDefault();

        console.log('Attempting to login...');
        console.log(this.state);
        const formData = new FormData();
        formData.append('username', 'password');
        formData.append('email', this.state.email);
        formData.append('password', this.state.password);

        console.log(this.state.email);
        console.log(this.state.password);

        // HTTP Get Request
        await fetch('https://localhost:49200/gateway/AccountLogin')
            .then(response => console.log(response.text()))
            .then(data => console.log(data));

        // HTTP Post Request
        await fetch('https://localhost:49200/gateway/AccountLogin', {
            method: 'POST',
            body: formData,
        }).then(function (response) {
            console.log(response.status); // returns 200;
        });
    }

    render() {
        return (
            <div className="Login">
                <header className="Login-header">
                    <div className="Login-box">
                        <label className="label-1">Login</label>
                        <form className="Login-form" onSubmit={this.sendLogin}>
                            <section className="Login-section">
                                <label>Email:</label>
                                <input className="Login-input" onChange={this.handleChange} value={this.state.email}
                                       type="email" name="email" placeholder="email"/>
                            </section>
                            <section className="Login-section">
                                <label>Password:</label>
                                <input className="Login-input" onChange={this.handleChange} value={this.state.password}
                                       type="password" name="password" placeholder="password"/>
                            </section>
                            <input type="submit" value="Submit"/>
                        </form>
                        <a className="Login-link link-1" href="https://reactjs.org">
                            Forgot your password?
                        </a>
                    </div>
                    <a className="Login-link" href="https://reactjs.org">
                        Continue as Guest
                    </a>
                </header>
            </div>
        );
    }
}

export default Login;
