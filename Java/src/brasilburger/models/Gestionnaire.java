package brasilburger.models;

public class Gestionnaire {
    private int idGestionnaire;
    private String login;
    private String password;

    public Gestionnaire() {
    }

    public Gestionnaire(int idGestionnaire, String login, String password) {
        this.idGestionnaire = idGestionnaire;
        this.login = login;
        this.password = password;
    }

    public int getIdGestionnaire() {
        return idGestionnaire;
    }

    public void setIdGestionnaire(int idGestionnaire) {
        this.idGestionnaire = idGestionnaire;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}

