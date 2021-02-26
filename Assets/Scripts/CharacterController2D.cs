using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
	public float jumpForce = 400f;
	[Range(0, .3f)] public float smoothing = .05f;
	public bool airControl = false;
	public LayerMask walkable;
	public LayerMask enemiesLayer;
	public Transform groundChecker;
	public Transform attackPoint;
	private float _groundedRadius = .2f;
	public float attackRadius = .6f;
	public bool isGrounded;
	private Rigidbody2D _rb;
	private bool _isLookingTo = true;
	private Vector3 _velocity = Vector3.zero;
	public float life;
	public float maxLife;
	public float damage;
	public float attackCooldown=1f;
	public float recharging;
	private SpriteRenderer _sprite;
	public GameObject bullet;
	public bool appleCollected;
	public float speed;
	public GameObject dieParticle;
	public bool growAppleCollected;
	
	private Animator _animator;

	public AudioSource hitSound;
	public AudioSource slash;
	public AudioSource hop;
	public AudioSource poof;
	
	//Enemy
	public Vector3 pointA = new Vector3(1, 0, 0);
	public Vector3 pointB = new Vector3(-1, 0, 0);
	private float movementTime = 3;
	private float t;
	public float lookRadius = 5f;
	private Transform _player;
	public bool itMoves;
	public GameObject drop;
	public RectTransform[] lifeBarSprite;
	public LayerMask mask;
	public bool isRanged;
	
	
	//Boss
	public GameObject[] spawns;
	public GameObject enemy1;
	public GameObject enemy2;
	public int attackCount;
	public AudioSource laugh;
	private float _spawnCooldown = 18f;
	private float _spawnRecharging;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			_player = GameObject.FindGameObjectWithTag("Player").transform;
		}

		if (gameObject.name == "Enemy2" || gameObject.name == "Enemy2(Clone)")
		{
			isRanged = true;
		}
	}

	private void Start()
	{
		life = maxLife;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(attackPoint.transform.position,attackRadius);
		var enemy = GetComponent<Enemy>();
		if (enemy != null)
		{
			Gizmos.DrawWireSphere(transform.position,lookRadius);
		}
	}

	private void Update()
	{
		recharging = Mathf.Clamp(recharging, 0, attackCooldown);
		recharging -= Time.deltaTime;
		_spawnRecharging = Mathf.Clamp(_spawnRecharging, 0, _spawnCooldown);
		_spawnRecharging -= Time.deltaTime;
		t += Time.deltaTime;
		
		if (life <= 0)
		{
			_animator.SetTrigger("Die");
			GameObject diepart = Instantiate(dieParticle, transform.position, transform.rotation);
			Destroy(diepart,2f);
			poof.Play();
			Destroy(gameObject,2f);
			if (GetComponent<Enemy>() != null)
			{
				Instantiate(drop, transform.position, transform.rotation);
			}
			Destroy(this);
		}
	}

	private void FixedUpdate()
	{
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundChecker.position, _groundedRadius, walkable);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				isGrounded = true;
		}

	}


	public void Move(float move)
	{
		if (isGrounded || airControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
			_rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, smoothing);

			if (move > 0 && !_isLookingTo)
			{
				Flip();
			}
			else if (move < 0 && _isLookingTo)
			{
				Flip();
			}
		}
	}

	public void EnemyMove()
	{
		if (itMoves)
		{
			Vector3 targetVelocity = Vector3.Lerp(pointA, pointB, t * speed);
			_rb.velocity = new Vector2(targetVelocity.x, _rb.velocity.y);
			if (t*speed>=speed*movementTime)
			{
				Vector3 b = pointB;
				Vector3 a = pointA;
				pointA = b;
				pointB = a;
				t = 0;
			}
		}
		
		if (_rb.velocity.x < 0 && _isLookingTo)
		{
			Flip();
		}else if (_rb.velocity.x > 0 && !_isLookingTo)
		{
			Flip();
		}

		_animator.SetFloat("xMove",_rb.velocity.x);
	}

	public void Pursuit()
	{
		if (_player != null)
		{
			float distance = Vector3.Distance(_player.position, transform.position);
			if (distance<=lookRadius)
			{
				itMoves = false;
				Vector3 targetVelocity = (_player.position - transform.position).normalized * speed;
				_rb.velocity = new Vector2(targetVelocity.x,_rb.velocity.y);
				if (distance <= 2f && isRanged == false)
				{
					_animator.SetBool("Walk",false);
					_rb.velocity = Vector3.zero;
					Attack();
					
					RaycastHit2D hit = Physics2D.Raycast(attackPoint.position,attackPoint.right, distance, mask);
					if (hit == false)
					{ 
						itMoves = true;
					}
				}else if (distance <= lookRadius - .2f && isRanged)
				{
					_animator.SetBool("Walk",false);
					_rb.velocity = Vector3.zero;
					SecondaryAttack();
					
					RaycastHit2D hit = Physics2D.Raycast(attackPoint.position,attackPoint.right, distance, mask);
					if (hit == false)
					{ 
						itMoves = true;
					}
				}
				
				
			}else if (distance>=lookRadius)
			{
				itMoves = true;
			}
		}
	}

	public void Jump(bool jump)
	{
		if (isGrounded && jump)
		{
			isGrounded = false;
			_animator.SetTrigger("Jump");
			hop.Play();
			_rb.AddForce(new Vector2(0f, jumpForce));
		}
	}

	public void Attack()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemiesLayer);
		if (recharging<= 0)
		{
			_animator.SetTrigger("Attack");
			slash.Play();
			foreach (Collider2D target in colliders)
			{
				if (target.GetComponent<CharacterController2D>() != null)
				{
					target.GetComponent<CharacterController2D>().TakeDamage(damage);
				}
			}
			
			recharging = attackCooldown;
		}
	}

	public void TakeDamage(float damage)
	{
		_animator.SetTrigger("TakeDamage");
		hitSound.Play();
		StartCoroutine(ReceiveDamageCoroutine(0.2f));
		life -= damage;
	}

	private void Flip()
	{
		_isLookingTo = !_isLookingTo;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		attackPoint.RotateAround(attackPoint.position, attackPoint.up,180f);

		if (GetComponent<Enemy>() != null)
		{
			foreach (var sprite in lifeBarSprite)
			{		
				Vector3 scale = sprite.localScale;
				scale.x *= -1;
				sprite.localScale = scale;
			}
		}

	}
	
	IEnumerator ReceiveDamageCoroutine(float seconds)
	{
		_sprite.color = Color.red;
		yield return new WaitForSeconds(seconds);
		_sprite.color = Color.white;
	}

	public void SecondaryAttack()
	{
		if (GetComponent<Player>() != null && appleCollected)
		{
			if (recharging <= 0)
			{
				_animator.SetTrigger("Attack");
				GameObject newBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
				newBullet.GetComponent<Bullet>().attackPoint = attackPoint;
				recharging = attackCooldown;
			}
		}else if (isRanged)
		{
			if (recharging <= 0)
			{
				_animator.SetTrigger("Attack");
				GameObject newBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
				newBullet.GetComponent<Bullet>().attackPoint = attackPoint;
				recharging = attackCooldown;
			}
		}
	}

	public void Grow()
	{
		transform.localScale = new Vector3(0.6799413f,0.6799413f,0.6799413f);
		maxLife = 300;
		damage = 20;
	}

	public void EnableSecondaryAttack()
	{
		appleCollected = true;
	}
	
	//BOSS
	public IEnumerator Boss()
	{
		while (attackCount != 3)
		{
			if (attackCount % 2 == 0)
			{
				if (_spawnRecharging<= 0)
				{
					foreach (var spawn in spawns)
					{
						_animator.SetTrigger("Spell");
						laugh.Play();
						Instantiate(enemy1, spawn.transform.position, spawn.transform.rotation);
						_spawnRecharging = _spawnCooldown;
					}
					attackCount++;
				}
			}else if (attackCount % 2 != 0)
			{
				if (_spawnRecharging <= 0)
				{
					foreach (var spawn in spawns)
					{
						_animator.SetTrigger("Spell");
						laugh.Play();
						Instantiate(enemy2, spawn.transform.position, spawn.transform.rotation);
						_spawnRecharging = _spawnCooldown;
					}
					attackCount++;
				}
			}
			yield return new WaitForSeconds(_spawnCooldown);
		}
	}
}
