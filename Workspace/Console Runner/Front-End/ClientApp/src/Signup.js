import './Signup.css';
import React from 'react';

class Signup extends React.Component {
    constructor(props) {
        super(props);
        this.state = {name: '', email: '', password: '', confirmPassword: ''};
        this.handleChange = this.handleChange.bind(this);
        this.sendLogin = this.sendLogin.bind(this);
        //this.email = React.createRef();
        //this.password = React.createRef();
    }

    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }

    async sendSignup(e) {
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
            <div className="Signup">
                <header className="Signup-header">
                    <div className="Signup-box">
                        <label className="label-1">Signup</label>
                        <form className="Signup-form" onSubmit={this.sendSignup}>
                            <section className= "Signup-section">
                                <label>Name:</label>
                                <input className="Signup-input" onChange={this.handleChange.name} 
                                    value={this.state.name} type="text" name="name" 
                                    placeholder='First and Last Name'/>
                            </section>
                            <section className="Signup-section">
                                <label>Email:</label>
                                <input className="Signup-input" onChange={this.handleChange} value={this.state.email}
                                       type="email" name="email" placeholder="email"/>
                            </section>
                            <section className="Signup-section">
                                <label>Password:</label>
                                <input className="Signup-input" onChange={this.handleChange} value={this.state.password}
                                       type="password" name="password" placeholder="password"/>
                            </section>
                            <section className="Signup-section">
                                <label>Confirm Password:</label>
                                <input className="Signup-input" onChange={this.handleChange} value={this.state.confirmPassword}
                                       type="confirm password" name="confirm password" placeholder="confirm password"/>
                            </section>
                            <input type="submit" value="Submit"/>
                        </form>
                    </div>
                </header>
            </div>
        );
    }
}

export default Signup;
