CREATE TABLE HistorialExpediente (
    Historial_ID INT PRIMARY KEY IDENTITY,
    Expediente_ID INT NOT NULL,
    Usuario_ID INT NOT NULL,
    Fecha_Modificacion DATETIME NOT NULL,
    Descripcion VARCHAR(500),
    EstadoNuevo VARCHAR(100),
    FOREIGN KEY (Expediente_ID) REFERENCES Expediente(Expediente_ID),
    FOREIGN KEY (Usuario_ID) REFERENCES Usuario(Usuario_ID)
);
