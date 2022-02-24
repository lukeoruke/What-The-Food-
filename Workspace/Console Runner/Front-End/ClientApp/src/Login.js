import './Login.css';


function Login() {
    return (
        <div className="Login">
            <header className="Login-header">
                <div className="Login-box">
                    <label className="label-1">Login</label>
                    <form className="Login-form">
                        <section className="Login-section">
                            <label>Email:</label>
                            <input className="Login-input" type="email" name="Email" placeholder="email"/>
                        </section>
                        <section className="Login-section">
                            <label>Password:</label>
                            <input className="Login-input" type="password" name="Password" placeholder="password"/>
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

export default Login;
