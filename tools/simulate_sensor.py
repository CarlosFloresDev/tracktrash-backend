"""Simulador explícito del protocolo HTTP del ESP32. Solo utiliza datos de prueba."""
import argparse
import datetime as dt
import json
import os
import urllib.error
import urllib.request


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--api', default='http://localhost:5080')
    parser.add_argument('--device', required=True, help='UUID obtenido al registrar el dispositivo')
    parser.add_argument('--sequence', type=int, required=True, help='Secuencia persistente; repetirla demuestra idempotencia')
    parser.add_argument('--distance', type=float, required=True, help='Distancia de prueba en cm')
    parser.add_argument('--battery', type=int, required=True, help='Porcentaje simulado, 0–100')
    args = parser.parse_args()
    key = os.environ.get('TRACKTRASH_DEVICE_KEY')
    if not key:
        parser.error('Configura TRACKTRASH_DEVICE_KEY; no pases claves en el historial de comandos.')
    body = {
        'secuencia': args.sequence,
        'distanciasCm': [args.distance, args.distance, args.distance],
        'bateria': args.battery,
        'medidaEn': dt.datetime.now(dt.timezone.utc).isoformat(),
    }
    request = urllib.request.Request(
        f'{args.api.rstrip("/")}/api/v1/iot/dispositivos/{args.device}/lecturas',
        data=json.dumps(body).encode(),
        headers={'Content-Type': 'application/json', 'X-Device-Key': key},
        method='POST',
    )
    try:
        with urllib.request.urlopen(request, timeout=15) as response:
            print(response.status, response.read().decode())
    except urllib.error.HTTPError as error:
        print(error.code, error.read().decode())
        raise SystemExit(1)


if __name__ == '__main__':
    main()
